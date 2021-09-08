# MAP-Elites Enemy Generator

This program is an Enemy Generator that evolves enemies via a MAP-Elites Genetic
Algorithm.
The output of this program is a set of enemies written in JSON files.

**ATTENTION:**
This program was designed for the game prototype of the Game Research Group of
Universidade de São Paulo (USP).
Therefore, the designed enemy representation and difficulty function may not
work for other games.

This enemy generator receives seven arguments:
- [Optional] the save separately flag (`-s`);
  * if the flag `-s` is entered,  then the enemies on the final population
    will be written separately, each one in a single JSON file.
- a random seed;
- the number of generations;
- the initial population size;
- the mutation chance;
- the crossover chance, and;
- the number of tournament competitors.