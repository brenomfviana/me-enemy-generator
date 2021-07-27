import os
import sys
from pathlib import Path
import json
import numpy as np
import seaborn as sb
from pandas import DataFrame
import matplotlib.pyplot as plt

CHART_FOLDER = 'charts'

# Check if the folder exists and create it if it does not exist
if not os.path.isdir(CHART_FOLDER):
  os.mkdir(CHART_FOLDER)

# List of indexes
movements = [
  'None', 'Random', 'Follow', 'Flee', 'Random1D', 'Follow1D', 'Flee1D'
]
# List of columns
weapons = ['None', 'Sword', 'Shotgun', 'Cannon', 'Shield']

def to_map(array, attribute):
  shape = (len(movements), (len(weapons)))
  map = np.zeros(shape)
  i = 0
  for b in range(len(movements)):
    for w in range(len(weapons)):
      if array[i] is None:
        map[b, w] = np.nan
      else:
        map[b, w] = array[i][attribute]
      i += 1
  return map


def plot_heatmap(map, folder, filename, pop, max):
  df = DataFrame(map, index=movements, columns=weapons)
  color = sb.color_palette('viridis_r', as_cmap=True)
  sb.heatmap(df, vmin=-1, vmax=max, annot=True, cmap=color)
  figname = folder + '/' + filename + '-' + pop + '.png'
  plt.savefig(figname)
  plt.close()

def plot(data, filename, folder):
  # Parse file
  obj = json.loads(data)

  filename = filename.replace('.json', '')
  folder = folder +  '/' + filename
  if not os.path.isdir(folder):
    os.mkdir(folder)

  # Fitness heatmap of the populations
  fit_map_initial = to_map(obj['initial'], 'fitness')
  fit_map_intermediate = to_map(obj['intermediate'], 'fitness')
  fit_map_final = to_map(obj['solution'], 'fitness')

  plot_heatmap(fit_map_initial, folder, filename, 'fitness_initial', 18)
  plot_heatmap(fit_map_intermediate, folder, filename, 'fitness_intermediate', 18)
  plot_heatmap(fit_map_final, folder, filename, 'fitness_final', 18)

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
  filename = folder + '/' + filename + '-measure.json'

  # Writing to sample.json
  with open(filename, "w") as outfile:
    outfile.write(json_object)


# Calculate the filename
folder = 'results/'
param_folder = sys.argv[2] + '-'  # Number of generations
param_folder += sys.argv[3] + '-' # Initial population size
param_folder += sys.argv[4] + '-' # Offspring size
param_folder += sys.argv[5]       # Desired fitness


# Read all files form this folder
files = []
filenames = []
for p in Path(folder + param_folder).glob('*.json'):
  with p.open() as f:
    files.append(f.read())
    filenames.append(p.name)

folder = CHART_FOLDER + '/' + param_folder
if not os.path.isdir(folder):
  os.mkdir(folder)

for i in range(len(files)):
  plot(files[i], filenames[i], folder)