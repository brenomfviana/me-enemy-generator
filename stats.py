import os
from pathlib import Path
import json
import numpy as np
from pandas import DataFrame


# List of indexes
movement = [
  'None', 'Random', 'Follow', 'Flee', 'Random1D', 'Follow1D', 'Flee1D'
]
# List of columns
weapon = [
  'Barehand', 'Sword', 'Bow', 'Bomb Thrower', 'Shield', 'Cure Spell'
]


# Convert the list of files to a map
def to_map(array, attribute):
  shape = (len(movement), len(weapon))
  map = np.zeros(shape)
  i = 0
  for b in range(len(movement)):
    for w in range(len(weapon)):
      if array[i] is None:
        map[b, w] = None
      else:
        map[b, w] = array[i][attribute]
      i += 1
  return map

shape = (len(movement), len(weapon))
counter_map = np.zeros(shape)
mean_map = np.zeros(shape)
std_map = np.zeros(shape)

amount = 100

maps = []

path = 'results' + os.path.sep + '500-35-100-20-30-2-18' + os.path.sep
for p in Path(path).glob('*.json'):
  with p.open() as f:
    # Convert the read files into a map of fitness
    # and add them to the list of maps
    obj = json.loads(f.read())
    maps.append(to_map(obj['final'], 'fitness'))


# Count the number of Elites
for map in maps:
  for l in range(len(movement)):
    for e in range(len(weapon)):
      if not np.isnan(map[l, e]):
        counter_map[l, e] += 1

# Uncomment to debug the counter map
for l in range(len(movement)):
  for e in range(len(weapon)):
    print('%3.2f' % counter_map[l, e], end=' ')
  print()
print()


# Calculate the mean map
for map in maps:
  for l in range(len(movement)):
    for e in range(len(weapon)):
      if not np.isnan(map[l, e]):
        mean_map[l, e] += map[l, e]

for l in range(len(movement)):
  for e in range(len(weapon)):
    if counter_map[l, e] > 0:
      mean_map[l, e] = mean_map[l, e] / counter_map[l, e]
    else:
      mean_map[l, e] = -1

# # Uncomment to debug the mean map
# for l in range(len(movement)):
#   for e in range(len(weapon)):
#     print('%3.2f' % mean_map[l, e], end=' ')
#   print()
# print()

for map in maps:
  for l in range(len(movement)):
    for e in range(len(weapon)):
      if not np.isnan(map[l, e]):
        std_map[l, e] += pow(map[l, e] - mean_map[l, e], 2)

for l in range(len(movement)):
  for e in range(len(weapon)):
    if counter_map[l, e] > 0:
      std_map[l, e] = std_map[l, e] / counter_map[l, e]
    else:
      mean_map[l, e] = -1
std_map = np.sqrt(std_map)

# # Uncomment to debug the std map
# for l in range(len(movement)):
#   for e in range(len(weapon)):
#     print('%3.2f' % std_map[l, e], end=' ')
#   print()
# print()

mean_excelent = 0
mean_good = 0
mean_medium = 0
mean_bad = 0
mean_very_bad = 0

std_excelent = 0
std_good = 0
std_medium = 0
std_bad = 0
std_very_bad = 0

# Merge the mean and std maps
fmap = [ [ '' for e in range(len(weapon)) ] for l in range(len(movement)) ]
for l in range(len(movement)):
  for e in range(len(weapon)):
    fmap[l][e] = '{:.2f}+-{:.2f}'.format(mean_map[l, e], std_map[l, e])
    # Mean
    if (mean_map[l, e] < 0.05):
      mean_excelent += 1
    elif (mean_map[l, e] < 0.2):
      mean_good += 1
    elif (mean_map[l, e] < 0.5):
      mean_medium += 1
    elif (mean_map[l, e] < 1):
      mean_bad += 1
    else:
      mean_very_bad += 1
    # STD
    if (std_map[l, e] < 0.05):
      std_excelent += 1
    elif (std_map[l, e] < 0.2):
      std_good += 1
    elif (std_map[l, e] < 0.5):
      std_medium += 1
    elif (std_map[l, e] < 1):
      std_bad += 1
    else:
      std_very_bad += 1

print('mean_excelent:', mean_excelent)
print('mean_good:', mean_good)
print('mean_medium:', mean_medium)
print('mean_bad:', mean_bad)
print('mean_very_bad:', mean_very_bad)
print()

print('std_excelent:', std_excelent)
print('std_good:', std_good)
print('std_medium:', std_medium)
print('std_bad:', std_bad)
print('std_very_bad:', std_very_bad)
print()

# # Uncomment to debug the merged map
# for l in range(len(movement)):
#   for e in range(len(weapon)):
#     print(fmap[l][e], end=' ')
#   print()

# Print the resulting table
df = DataFrame(fmap, index=movement, columns=weapon)
print(df)

# # Uncomment to write a CSV file with the resulting table
# # filename = 'std_atual.csv'
# # df.to_csv(filename)