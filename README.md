

# The task "The life of wolves and hares"

### Practical assignment on parallel programming in c# (MAUI.NET)
## Task text:

A two-dimensional field of cells is given, each of
which either contains a wolf (1), or a hare (2), or is empty (0). Each cell
checks the state of its neighbors (there are 8 of them) and changes its own according to the rules:
1. Wolves and hares live by the rules:
- A living cell with < 2 living cells around it is dying of loneliness.
- A living cell with 2 or 3 living cells around it survives.
- A living cell with > 3 living cells around it is dying from overpopulation.
- An empty cell with 3 living neighbors next to it comes to life.
2. The wolf eats one neighbor hare, in any order of your choice, in one
step.
3. The order of the rules is also chosen by you (whether the wolf will eat faster
than the hare, or the hare will die by itself).
4. Use the ripple algorithm.