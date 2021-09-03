import os
import sys
from pathlib import Path
import json
import numpy as np
import seaborn as sb
from pandas import DataFrame
import matplotlib.pyplot as plt

CHART_FOLDER = 'charts'

# Check if the folder for charts exists and create it if it does not exist
if not os.path.isdir(CHART_FOLDER):
  os.mkdir(CHART_FOLDER)


# List of indexes
difficulty = ['8, 12', '12, 16', '16, 20', '20, 24', '24, 28']
# List of columns
weapons = ['Barehand', 'Sword', 'Bow', 'Bomb Thrower', 'Shield', 'Cure Spell']


def to_map(array, attribute):
  shape = (len(difficulty), (len(weapons)))
  map = np.zeros(shape)
  i = 0
  for b in range(len(difficulty)):
    for w in range(len(weapons)):
      if array[i] is None:
        map[b, w] = np.nan
      else:
        map[b, w] = array[i][attribute]
      i += 1
  return map


def plot_heatmap(map, folder, filename, pop, max):
  df = DataFrame(map, index=difficulty, columns=weapons)
  color = sb.color_palette('viridis_r', as_cmap=True)
  ax = sb.heatmap(df, vmin=0, vmax=max, annot=True, cmap=color)
  ax.invert_yaxis()
  figname = folder + os.path.sep + filename + '-' + pop + '.png'
  plt.subplots_adjust(bottom=0.3)
  plt.savefig(figname)
  plt.close()


def plot(data, filename, folder):
  # Parse file
  obj = json.loads(data)

  filename = filename.replace('.json', '')
  folder = folder +  os.path.sep + filename
  if not os.path.isdir(folder):
    os.mkdir(folder)

  # Fitness heatmap of the populations
  fit_map_initial = to_map(obj['initial'], 'fitness')
  fit_map_intermediate = to_map(obj['intermediate'], 'fitness')
  fit_map_final = to_map(obj['solution'], 'fitness')

  plot_heatmap(fit_map_initial, folder, filename, 'fitness_initial', 2)
  plot_heatmap(fit_map_intermediate, folder, filename, 'fitness_intermediate', 2)
  plot_heatmap(fit_map_final, folder, filename, 'fitness_final', 2)

  # # Generation heatmap of the populations
  # gen_map_initial = to_map(obj['initial'], 'generation')
  # gen_map_intermediate = to_map(obj['intermediate'], 'generation')
  # gen_map_final = to_map(obj['solution'], 'generation')

  # plot_heatmap(gen_map_initial, folder, filename, 'generation_initial', int(sys.argv[3]))
  # plot_heatmap(gen_map_intermediate, folder, filename, 'generation_intermediate', int(sys.argv[3]))
  # plot_heatmap(gen_map_final, folder, filename, 'generation_final', int(sys.argv[3]))


  # Print measurements
  measure = {}

  # Calculate mean fitness
  measure['mean_fit_map_initial'] = np.mean(fit_map_initial)
  measure['mean_fit_map_intermediate'] = np.mean(fit_map_intermediate)
  measure['mean_fit_map_final'] = np.mean(fit_map_final)

  # Calculate max fitness
  measure['max_fit_map_initial'] = np.max(fit_map_initial)
  measure['max_fit_map_intermediate'] = np.max(fit_map_intermediate)
  measure['max_fit_map_final'] = np.max(fit_map_final)

  # Calculate min fitness
  measure['min_fit_map_initial'] = np.min(fit_map_initial)
  measure['min_fit_map_intermediate'] = np.min(fit_map_intermediate)
  measure['min_fit_map_final'] = np.min(fit_map_final)

  # Calculate the standard deviation
  measure['std_fit_map_initial'] = np.std(fit_map_initial)
  measure['std_fit_map_intermediate'] = np.std(fit_map_intermediate)
  measure['std_fit_map_final'] = np.std(fit_map_final)

  # Serializing json
  json_object = json.dumps(measure, indent = 4)

  # Write JSON file
  filename = folder + os.path.sep + filename + '-measure.json'

  # Writing to sample.json
  with open(filename, "w") as outfile:
    outfile.write(json_object)


# Calculate the filename
folder = 'results' + os.path.sep
param_folder = sys.argv[2] + '-' # Number of generations
param_folder += sys.argv[3]      # Initial population size


# Read all files form this folder
files = []
filenames = []
for p in Path(folder + param_folder).glob('*.json'):
  with p.open() as f:
    files.append(f.read())
    filenames.append(p.name)

folder = CHART_FOLDER + os.path.sep + param_folder
if not os.path.isdir(folder):
  os.mkdir(folder)

for i in range(len(files)):
  plot(files[i], filenames[i], folder)