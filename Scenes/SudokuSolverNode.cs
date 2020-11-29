using Godot;
using System;
using System.Collections.Generic;
using SudokuSolver.CSP_Solver.Solver;
using SudokuSolver.CSP_Solver;
using SudokuSolver.CSP_Solver.Constraints;

public class SudokuSolverNode : Node
{
    private Solver solver;
    private Node sudokuGrid;
    private bool isSolving = false;
    private Dictionary<Variable, Node> relationVariablesSudoku;
    
    public override void _Ready()
    {
        solver = new BacktrackSolver();
        sudokuGrid = GetTree().Root.GetNode("MainScene/Interface/Center/VBox/Sudoku/SudokuGrid");
        loadTest(sudokuGrid);
    }

    private void loadTest(Node sudokuGrid)
    {
        Godot.Collections.Array grid = (Godot.Collections.Array)sudokuGrid.Call("export_grid");
        /*
        List<int>test= new List<int>(new int[] {
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        4, 5, 6, 7, 8, 9, 1, 2, 3,
        7, 8, 9, 1, 2, 3, 4, 5, 6,
        2, 3, 4, 5, 6, 7, 8, 9, 1,
        5, 6, 7, 8, 9, 1, 2, 3, 4,
        8, 9, 1, 2, 3, 4, 5, 6, 7,
        3, 4, 5, 6, 7, 8, 9, 1, 2,
        6, 7, 8, 9, 1, 2, 3, 4, 5,
        9, 1, 2, 3, 4, 5, 6, 7, 8,
        });
        */
        
        List<int>test= new List<int>(new int[] {
        1, 2, 3, 4, 5, 6, 0, 8, 9,
        4, 5, 0, 7, 0, 9, 1, 0, 3,
        7, 8, 0, 1, 2, 3, 0, 0, 6,
        0, 0, 4, 5, 6, 7, 8, 9, 1,
        5, 6, 0, 8, 0, 1, 2, 3, 0,
        8, 9, 0, 2, 3, 4, 5, 6, 7,
        3, 4, 5, 6, 0, 8, 9, 1, 2,
        6, 7, 0, 9, 1, 2, 3, 0, 5,
        0, 1, 2, 3, 4, 5, 6, 7, 8,
        });
        
        int i = 0;
        foreach(Godot.Collections.Array row in grid)
        {
            foreach(Node cell in row)
            {
                cell.Call("select", test[i]);
                ++i;
            }
        }
    }

    public void OnButtonPressed()
    {
        if (isSolving)
            return;
        isSolving = true;
        Godot.Collections.Array grid = (Godot.Collections.Array) sudokuGrid.Call("export_grid");

        Variable[,] variables = CreateVariables(grid);
        Assignment initial_assignment = createAssignment(variables);
        List<Constraint> constraints = createConstraints(variables);

        List<Variable> listOfVariables = new List<Variable>();
        relationVariablesSudoku = new Dictionary<Variable, Node>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                relationVariablesSudoku.Add(variables[i, j], (Node)((Godot.Collections.Array)grid[i])[j]);
                listOfVariables.Add(variables[i, j]);
            }
        }

        
        Assignment solution = solver.solve(new ConstrainSatisfactionProblem(listOfVariables, constraints), initial_assignment);

        if (solution != null)
        {
            GD.Print("solved!");
            displayResults(solution);
        }
        else
        {
            GD.Print("No solution found");
        }
    }

    private void displayResults(Assignment solution)
    {
        foreach(Variable variable in relationVariablesSudoku.Keys)
        {
            Node cell;
            relationVariablesSudoku.TryGetValue(variable, out cell);
            int number =  (int)solution.valueOf(variable);
            cell.Call("select", number);
        }
    }

    private List<Constraint> createConstraints(Variable[,] v)
    {
        List<Constraint> constraints = new List<Constraint>();

        List<Variable> row = new List<Variable>();
        for (int i = 0; i < 9; i++)
        {
            row.Clear();
            for (int j = 0; j < 9; j++)
            {
                row.Add(v[i, j]);
            }
            constraints.AddRange(allDiff(row));
        }


        List<Variable> column = new List<Variable>();
        for (int i = 0; i < 9; i++)
        {
            column.Clear();
            for (int j = 0; j < 9; j++)
            {
                column.Add(v[j, i]);
            }
            constraints.AddRange(allDiff(column));
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                constraints.AddRange(allDiff(variablesOfBoxStartingAt(v, (i%3)*3, j*3)));
            }
        }

        return constraints;
    }

    private List<Variable> variablesOfBoxStartingAt(Variable[,] v, int row, int column)
    {
        List<Variable> variables = new List<Variable>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                variables.Add(v[row + i, column + j]);
            }
        }
        return variables;
    }

    private Variable[,] CreateVariables(Godot.Collections.Array grid)
    {
        Variable[,] variables = new Variable[9, 9];
        for (int i = 0; i < 9; ++i)
        {
            Godot.Collections.Array row = (Godot.Collections.Array)grid[i];
            for (int j = 0; j < 9; ++j)
            {
                int[] values;
                Node cell = (Node)row[j];
                int cell_value = (int)cell.Call("get_selected_id");
                if (cell_value == 0)
                {
                    values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
                else
                {
                    values = new int[] { cell_value };
                }
                HashSet<object> hashSet = new HashSet<object>();
                foreach (int n in values)
                {
                    hashSet.Add(n);
                }

                variables[i, j] = new Variable(new Domain(hashSet));
            }
        }
        return variables;
    }
    private Assignment createAssignment(Variable[,] variables)
    {
        Assignment assignment = new Assignment();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                List<object> list = new List<object>(variables[i, j].getDomain().getValues());
                if (list.Count == 1)
                    assignment.assign(variables[i, j], list[0]);
            }
        }
        return assignment;
    }

    private List<Constraint> allDiff(List<Variable> variables)
    {
        List<Constraint> binaryConstraints = new List<Constraint>();
        for (int i = 0; i < variables.Count ; i++)
        {
            for (int j = i+1; j < variables.Count; j++)
            {
                binaryConstraints.Add(new BinaryNotEquals(variables[i], variables[j]));
            }
        }
        
        return binaryConstraints;
    }

    private List<Constraint> allDiff(params Variable[] variables)
    {
        return allDiff(new List<Variable>(variables));
    }
}
