# MAP-Elites Enemy Generator

This program is an Enemy Generator that evolves enemies via a MAP-Elites GA.
The result of this program is a set of enemies written in JSON files.

**ATTENTION:** This program was designed for the game prototype of the Game Research
Group of Universidade de São Paulo (USP). Therefore, the designed enemy
representation and difficulty function may not work for other games.

This enemy generator receives five arguments:
- (Optional) save separately flag (`-s`);
  * if the flag `-s` is inputted, then only the enemies of the final
    population will be saved.
- a random seed;
- the number of generations;
- the initial population size;
- the mutation chance, and;
- the crossover chance.