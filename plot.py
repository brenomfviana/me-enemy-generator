import os
import sys
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
behavior = ['Indifferent', 'Lone Wolf', 'Swarm']
# List of columns
weapons = ['None', 'Sword', 'Shotgun', 'Cannon', 'Shield', 'Cure']

def to_map(array, attribute):
  shape = (len(behavior), (len(weapons)))
  map = np.zeros(shape)
  i = 0
  for b in range(len(behavior)):
    for w in range(len(weapons)):
      if array[i] is None:
        map[b, w] = np.nan
      else:
        map[b, w] = array[i][attribute]
      i += 1
  return map


def plot_heatmap(map, filename, pop, max):
  df = DataFrame(map, index=behavior, columns=weapons)
  color = sb.color_palette('viridis_r', as_cmap=True)
  sb.heatmap(df, vmin=-1, vmax=max, annot=True, cmap=color)
  figname = filename.replace('results', CHART_FOLDER)
  figname = figname.replace('.json', '-' + pop + '.png')
  plt.savefig(figname)
  plt.close()


# Calculate the filename
filename = 'results/'
filename += sys.argv[3] + '-' # Number of generations
filename += sys.argv[4] + '-' # Number of individuals of the initial population
filename += sys.argv[5] + '-' # Number of individuals of offspring
filename += sys.argv[1] + '.json'

# Read JSON file
with open(filename, 'r') as json_file:
  data = json_file.read()

# Parse file
obj = json.loads(data)

# Fitness heatmap of the populations
fit_map_initial = to_map(obj['initial'], 'fitness')
fit_map_intermediate = to_map(obj['intermediate'], 'fitness')
fit_map_final = to_map(obj['solution'], 'fitness')

plot_heatmap(fit_map_initial, filename, 'fitness_initial', 35)
plot_heatmap(fit_map_intermediate, filename, 'fitness_intermediate', 35)
plot_heatmap(fit_map_final, filename, 'fitness_final', 35)

# Generation heatmap of the populations
gen_map_initial = to_map(obj['initial'], 'generation')
gen_map_intermediate = to_map(obj['intermediate'], 'generation')
gen_map_final = to_map(obj['solution'], 'generation')

plot_heatmap(gen_map_initial, filename, 'generation_initial', int(sys.argv[3]))
plot_heatmap(gen_map_intermediate, filename, 'generation_intermediate', int(sys.argv[3]))
plot_heatmap(gen_map_final, filename, 'generation_final', int(sys.argv[3]))


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
filename = filename.replace('results', CHART_FOLDER)
filename = filename.replace('.json', '-measure.json')

# Writing to sample.json
with open(filename, "w") as outfile:
  outfile.write(json_object)