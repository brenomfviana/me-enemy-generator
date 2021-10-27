import os
import sys
from pathlib import Path
import copy
import json
import numpy as np
import seaborn as sb
from pandas import DataFrame
import matplotlib.pyplot as plt


# List of indexes
movement = [
  'None', 'Random', 'Follow', 'Flee', 'Random1D', 'Follow1D', 'Flee1D'
]
# List of columns
weapon = [
  'Barehand', 'Sword', 'Bow', 'Bomb Thrower', 'Shield', 'Cure Spell'
]


# Convert the array into a map
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


# Plot the heatmap
def plot_heatmap(map, folder, filename, pop, max):
  df = DataFrame(map, index=movement, columns=weapon)
  color = sb.color_palette('viridis_r', as_cmap=True)
  ax = sb.heatmap(df, vmin=0, vmax=max, annot=True, cmap=color)
  ax.invert_yaxis()
  figname = folder + os.path.sep + filename + '-' + pop + '.png'
  plt.subplots_adjust(bottom=0.3)
  plt.savefig(figname)
  plt.close()


# Plot all charts
def plot_charts(data, filename, folder):
  # Parse file
  obj = json.loads(data)

  # Create the folder for the entered parameters
  filename = filename.replace('.json', '')
  folder = folder +  os.path.sep + filename
  if os.path.isdir(folder):
    return
  os.mkdir(folder)

  # Fitness heatmap of the populations
  attribute = 'fitness'
  f_initial = to_map(obj['initial'], attribute)
  f_intermediate = to_map(obj['intermediate'], attribute)
  f_final = to_map(obj['final'], attribute)

  map_aux = copy.deepcopy(f_initial)
  for l in range(len(movement)):
    for e in range(len(weapon)):
      if np.isnan(map_aux[l, e]):
        map_aux[l, e] = 0
  max = map_aux.max()
  plot_heatmap(f_initial, folder, filename, 'fitness_initial', max)
  plot_heatmap(f_intermediate, folder, filename, 'fitness_intermediate', max)
  plot_heatmap(f_final, folder, filename, 'fitness_final', max)



# Create the folder for the charts
CHART_FOLDER = 'charts'
if not os.path.isdir(CHART_FOLDER):
  os.mkdir(CHART_FOLDER)


# Calculate the filename
folder = 'results' + os.path.sep
param_folder = sys.argv[1] + '-'  # Number of generations
param_folder += sys.argv[2] + '-' # Initial population size
param_folder += sys.argv[3] + '-' # Intermediate population size
param_folder += sys.argv[4] + '-' # Mutation chance
param_folder += sys.argv[5] + '-' # Crossover chance
param_folder += sys.argv[6] + '-' # Number of competitors
param_folder += sys.argv[7]       # Aimed difficulty of enemies


# Read all the JSON files for the entered parameters
files = []
filenames = []
for p in Path(folder + param_folder).glob('*.json'):
  with p.open() as f:
    files.append(f.read())
    filenames.append(p.name)


# Create the folder for the entered parameters
folder = CHART_FOLDER + os.path.sep + param_folder
if not os.path.isdir(folder):
  os.mkdir(folder)


# Plot all the charts
for i in range(len(files)):
  plot_charts(files[i], filenames[i], folder)