using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SudokuManager : MonoBehaviour
{
    public GameObject cellPrefab; // Prefab for each cell
    public Transform gridParent;  // Parent object for the Sudoku grid
    public int[,] grid = new int[9, 9]; // Sudoku grid
    public TMP_InputField[,] inputFields = new TMP_InputField[9, 9]; // UI input fields for the grid

    private int[,] solution = new int[9, 9]; // Store the correct solution

    private void Start()
    {
        GenerateGrid();
        GenerateSudokuPuzzle();
    }

    void GenerateGrid()
    {
        // Instantiate the grid of input fields
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                TMP_InputField inputField = cell.GetComponent<TMP_InputField>();
                inputField.characterLimit = 1;
                inputField.onEndEdit.AddListener((text) => OnInputChanged(i, j, text));
                inputFields[i, j] = inputField;
            }
        }
    }

    void GenerateSudokuPuzzle()
    {
        // You could use a Sudoku generator here, but for simplicity, we use a fixed grid.
        int[,] puzzle = new int[9, 9]
        {
            { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
            { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
            { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
            { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
            { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
            { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
            { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
            { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
            { 0, 0, 0, 0, 8, 0, 0, 7, 9 }
        };

        // Solution for the puzzle
        solution = new int[9, 9]
        {
            { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
            { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
            { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
            { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
            { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
            { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
            { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
            { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
        };

        // Copy puzzle to the grid and set values in input fields
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = puzzle[i, j];
                if (puzzle[i, j] != 0)
                {
                    inputFields[i, j].text = puzzle[i, j].ToString();
                    inputFields[i, j].interactable = false;
                }
                else
                {
                    inputFields[i, j].text = "";
                    inputFields[i, j].interactable = true;
                }
            }
        }
    }

    public void OnInputChanged(int row, int col, string text)
    {
        if (int.TryParse(text, out int value))
        {
            grid[row, col] = value;

            if (IsValidMove(row, col, value))
            {
                inputFields[row, col].textComponent.color = Color.black;
            }
            else
            {
                inputFields[row, col].textComponent.color = Color.red; // Highlight invalid move
            }

            if (CheckForCompletion())
            {
                Debug.Log("Congratulations! You have completed the puzzle.");
            }
        }
    }

    bool IsValidMove(int row, int col, int value)
    {
        // Check if the value is already in the same row, column or 3x3 box
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == value && i != col) return false;
            if (grid[i, col] == value && i != row) return false;
        }

        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[boxRow + i, boxCol + j] == value && (boxRow + i != row || boxCol + j != col))
                    return false;
            }
        }

        return true;
    }

    bool CheckForCompletion()
    {
        // Check if the player has filled the entire grid correctly
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0 || grid[i, j] != solution[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
