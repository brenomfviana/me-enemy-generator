import os
from pathlib import Path
import json
import numpy as np
from pandas import DataFrame


# List of indexes
difficulty = ['8-12', '12-16', '16-20', '20-24', '24-28']
# List of columns
weapon = ['Barehand', 'Sword', 'Bow', 'Bomb Thrower', 'Shield', 'Cure Spell']


# Convert the list of files to a map
def to_map(array, attribute):
  shape = (len(difficulty), len(weapon))
  map = np.zeros(shape)
  i = 0
  for b in range(len(difficulty)):
    for w in range(len(weapon)):
      if array[i] is None:
        map[b, w] = None
      else:
        map[b, w] = array[i][attribute]
      i += 1
  return map

shape = (len(difficulty), len(weapon))
mean_map = np.zeros(shape)
std_map = np.zeros(shape)

maps = []

path = 'results' + os.path.sep + '100-25-30-80-3' + os.path.sep
for p in Path(path).glob('*.json'):
  with p.open() as f:
    # Convert the read files into a map of fitness
    # and add them to the list of maps
    obj = json.loads(f.read())
    maps.append(to_map(obj['final'], 'fitness'))

for map in maps:
  for l in range(len(difficulty)):
    for e in range(len(weapon)):
      print('%3.2f' % mean_map[l, e], end=' ')
      if not np.isnan(map[l, e]):
        mean_map[l, e] += map[l, e]
    print()
  print()

mean_map = mean_map / 10

# Uncomment to debug the mean map
# for l in range(len(difficulty)):
#   for e in range(len(weapon)):
#     print('%3.2f' % mean_map[l, e], end=' ')
#   print()

for map in maps:
  for l in range(len(difficulty)):
    for e in range(len(weapon)):
      if not np.isnan(map[l, e]):
        std_map[l, e] += pow(map[l, e] - mean_map[l, e], 2)
std_map = std_map / 10
std_map = np.sqrt(std_map)

# Uncomment to debug the std map
# for l in range(len(difficulty)):
#   for e in range(len(weapon)):
#     print('%3.2f' % std_map[l, e], end=' ')
#   print()

# Merge the mean and std maps
fmap = [ [ '' for e in range(len(weapon)) ] for l in range(len(difficulty)) ]
for l in range(len(difficulty)):
  for e in range(len(weapon)):
    fmap[l][e] = '{:.2f}+-{:.2f}'.format(mean_map[l, e], std_map[l, e])

# Uncomment to debug the merged map
# for l in range(len(difficulty)):
#   for e in range(len(weapon)):
#     print(fmap[l][e], end=' ')
#   print()

# Print the resulting table
df = DataFrame(fmap, index=difficulty, columns=weapon)
print(df)

# Uncomment to write a CSV file with the resulting table
# filename = 'std_atual.csv'
# df.to_csv(filename)