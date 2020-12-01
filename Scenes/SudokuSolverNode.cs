using Godot;
using System;
using System.Collections.Generic;
using SudokuSolver.CSP_Solver.Solver;
using SudokuSolver.CSP_Solver;
using SudokuSolver.CSP_Solver.Constraints;
using System.Threading;

public class SudokuSolverNode : Node
{
    private Solver<int> Solver;
    private Node sudokuGrid;
    private bool isSolving = false;
    private Dictionary<object, Node> relationVariablesSudoku;

    public override void _Ready()
    {
        Solver = new BacktrackSolver<int>();
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
        1, 2, 0, 0, 5, 6, 0, 8, 9,
        4, 5, 0, 7, 0, 9, 1, 0, 3,
        7, 8, 0, 1, 2, 3, 0, 0, 6,
        0, 0, 4, 5, 6, 0, 8, 9, 1,
        5, 6, 0, 8, 0, 1, 0, 3, 0,
        0, 9, 0, 0, 0, 4, 0, 6, 7,
        3, 0, 0, 6, 0, 8, 0, 1, 2,
        6, 7, 0, 0, 1, 2, 3, 0, 5,
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

        Solver.ClearEvents();

        Godot.Collections.Array grid = (Godot.Collections.Array)sudokuGrid.Call("export_grid");
        Variable<int>[,] variables = CreateVariablesInt(grid);
        GD.Print("variables created");

        Assignment<int> initial_assignment = CreateAssignment(variables);
        GD.Print("assignment created");

        List<Constraint<int>> constraints = CreateConstraints(variables);
        GD.Print("constraints created");

        relationVariablesSudoku = CreateMappingVariablesNodes(variables);
        GD.Print("mapping created");


        List<Variable<int>> variable_list = new List<Variable<int>>(9 * 9);
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                variable_list.Add(variables[i, j]);
            }
        }

        ConstraintSatisfactionProblem<int> csp = new ConstraintSatisfactionProblem<int>(variable_list, constraints);

        System.Threading.Thread thread = new System.Threading.Thread(() => StartSolvingSudoku(csp, initial_assignment));

        thread.Start();
    }

    private void StartSolvingSudoku(ConstraintSatisfactionProblem<int> csp, Assignment<int> initial_assignment)
    {
        Solver.SolutionSearchCompleate += SearchCompleteEventHandler;
        Solver.Solve(csp, initial_assignment);
    }

    private Variable<int>[,] CreateVariablesInt(Godot.Collections.Array grid)
    {
        Variable<int>[,] variables = new Variable<int>[9, 9];
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

                variables[i, j] = new Variable<int>(new Domain<int>(values),i+"-"+j);
            }
        }
        return variables;
    }
    private Assignment<Tval> CreateAssignment<Tval>(Variable<Tval>[,] variables)
    {
        Assignment<Tval> assignment = new Assignment<Tval>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                List<Tval> values = new List<Tval>(variables[i, j].GetDomain().GetValues());
                if (values.Count == 1)
                    assignment.Assign(variables[i, j], values[0]);
            }
        }
        return assignment;
    }

    private List<Constraint<Tval>> CreateConstraints<Tval>(Variable<Tval>[,] v)
    {
        List<Constraint<Tval>> constraints = new List<Constraint<Tval>>();

        // set all constraint for the rows
        for (int i = 0; i < 9; i++)
        {
            List<Variable<Tval>> row = new List<Variable<Tval>>(9);
            for (int j = 0; j < 9; j++)
            {
                row.Add(v[i, j]);
            }
            constraints.AddRange(AllDiff(row));
        }

        // set all constraint for the columns

        for (int i = 0; i < 9; i++)
        {
            List<Variable<Tval>> column = new List<Variable<Tval>>(9);
            for (int j = 0; j < 9; j++)
            {
                column.Add(v[j, i]);
            }
            constraints.AddRange(AllDiff(column));
        }

        // set all constraint for the sub-squares
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                constraints.AddRange(AllDiff(variablesOfBoxStartingAt(v, (i % 3) * 3, j * 3)));
            }
        }

        return constraints;
    }

    private IEnumerable<Constraint<Tval>> AllDiff<Tval>(List<Variable<Tval>> variables)
    {
        List<Constraint<Tval>> binaryConstraints = new List<Constraint<Tval>>(36);
        for (int i = 0; i < variables.Count; i++)
        {
            for (int j = i + 1; j < variables.Count; j++)
            {
                binaryConstraints.Add(new NotEquals<Tval>(variables[i], variables[j]));
            }
        }

        return binaryConstraints;
    }

    private List<Variable<Tval>> variablesOfBoxStartingAt<Tval>(Variable<Tval>[,] v, int row, int column)
    {
        List<Variable<Tval>> variables = new List<Variable<Tval>>(9);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                variables.Add(v[row + i, column + j]);
            }
        }
        return variables;
    }

    private Dictionary<object, Node> CreateMappingVariablesNodes<Tval>(Variable<Tval>[,] variables)
    {
        Dictionary<object, Node> mapping = new Dictionary<object, Node>(9 * 9);
        Godot.Collections.Array grid = (Godot.Collections.Array)sudokuGrid.Call("export_grid");

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                mapping.Add(variables[i, j], (Node)((Godot.Collections.Array)grid[i])[j]);
            }
        }

        return mapping;
    }

    private void SearchCompleteEventHandler<Tval>(object source, SolutionSearchCompleateEventArgs<Tval> eventArgs)
    {
        if (eventArgs.SolutionFound)
        {
            displayResults(eventArgs.Solution);
            GD.Print("Solution Found!");
        }
        else
        {
            GD.Print("No solution found");
        }

    }
    private void displayResults<Tval>(Assignment<Tval> solution)
    {
        foreach (Variable<Tval> variable in relationVariablesSudoku.Keys)
        {
            Node cell;
            relationVariablesSudoku.TryGetValue(variable, out cell);
            Tval number = solution.ValueOf(variable);
            cell.Call("select", number);
        }
    }
}
