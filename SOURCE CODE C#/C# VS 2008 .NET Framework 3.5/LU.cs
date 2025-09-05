
using System;
using System.Diagnostics;

namespace Graphical_2D_Frame_Analysis_CSharp
{
	internal static class LU
	{
#region 111-inverse LU


		


		public static void MatrixTest()
		{
			double[,] matrixA =
			{
				{1, 3, 3},
				{2, 4, 3},
				{1, 3, 4}
			};
            double[,] matrixB = Inverse(matrixA); //Inverse(matrixA)

			Debug.Print(MakeDisplayable(matrixA) + Environment.NewLine + Environment.NewLine + "Inverse: " + Environment.NewLine + MakeDisplayable(matrixB));
		}

		public static string MakeDisplayable(double[,] sourceMatrix)
		{
			// ----- Prepare a multi-line string that shows the contents
			//       of a matrix, a 2D array.
			int rows = 0;
			int cols = 0;
			int eachRow = 0;
			int eachCol = 0;
			System.Text.StringBuilder result = new System.Text.StringBuilder();

			// ----- Process all rows of the matrix, generating one
			//       output line per row.
			rows = sourceMatrix.GetUpperBound(0) + 1;
			cols = sourceMatrix.GetUpperBound(1) + 1;
			for (eachRow = 0; eachRow < rows; eachRow++)
			{
				// ----- Process each column of the matrix on a single
				//       row, separating values by commas.
				if (eachRow > 0)
				{
					result.AppendLine();
				}
				for (eachCol = 0; eachCol < cols; eachCol++)
				{
					// ----- Add a single matrix element to the output.
					if (eachCol > 0)
					{
						result.Append(",");
					}
					result.Append(sourceMatrix[eachRow, eachCol].ToString());
				}
			}

			// ----- Finished.
			return result.ToString();
		}

		public static string MakeDisplayable_1D(double[] sourceArray)
		{
			// ----- Present an array as multiple lines of output.
			System.Text.StringBuilder result = new System.Text.StringBuilder();

            //			Dim scanValue As Double

			foreach (double scanValue in sourceArray)
			{
				result.AppendLine(scanValue.ToString());
			}

			return result.ToString();
		}


		private static void ShowVector(int[] vector)
		{
			Console.Write("   ");
			for (var i = 0; i < vector.Length; i++)
			{
				Console.Write(vector[i] + " ");
			}
			Debug.Print(Environment.NewLine);
		}






		public static double[,] Inverse(double[,] sourceMatrix)
		{
			// ----- Build a new matrix that is the mathematical inverse
			//       of the supplied matrix. Multiplying a matrix and its
			//       inverse together will give the identity matrix.
			int eachCol = 0;
			int eachRow = 0;
			int rowsAndCols = 0;

			// ----- Determine the size of each dimension of the matrix.
			//       Only square matrices can be inverted.
			if (sourceMatrix.GetUpperBound(0) != sourceMatrix.GetUpperBound(1))
			{
				throw new Exception("Matrix must be square.");
			}
			int rank = sourceMatrix.GetUpperBound(0);

			// ----- Clone a copy of the matrix (not just a new reference).
			double[,] workMatrix = (double[,])sourceMatrix.Clone();

			// ----- Variables used for backsolving.
			double[,] destMatrix = new double[rank + 1, rank + 1];
			double[] rightHandSide = new double[rank + 1];
			double[] solutions = new double[rank + 1];
			int[] rowPivots = new int[rank + 1];
			int[] colPivots = new int[rank + 1];

			// ----- Use LU decomposition to form a triangular matrix.
			workMatrix = FormLU(workMatrix, ref rowPivots, ref colPivots, ref rowsAndCols);

			// ----- Backsolve the triangular matrix to get the inverted
			//       value for each position in the final matrix.
			for (eachCol = 0; eachCol <= rank; eachCol++)
			{
				rightHandSide[eachCol] = 1;
				BackSolve(workMatrix, rightHandSide, solutions, ref rowPivots, ref colPivots);
				for (eachRow = 0; eachRow <= rank; eachRow++)
				{
					destMatrix[eachRow, eachCol] = solutions[eachRow];
					rightHandSide[eachRow] = 0;
				}
			}

			// ----- Return the inverted matrix result.
			return destMatrix;
		}

		public static double Determinant(double[,] sourceMatrix)
		{
			// ----- Calculate the determinant of a matrix.
			double result = 0;
			int pivots = 0;
			int count = 0;

			// ----- Only calculate the determinants of square matrices.
			if (sourceMatrix.GetUpperBound(0) != sourceMatrix.GetUpperBound(1))
			{
				throw new Exception("Determinant only calculated for square matrices.");
			}
			int rank = sourceMatrix.GetUpperBound(0);

			// ----- Make a copy of the matrix so we can work inside of it.
			double[,] workMatrix = new double[rank + 1, rank + 1];
			Array.Copy(sourceMatrix, workMatrix, sourceMatrix.Length);

			// ----- Use LU decomposition to form a triangular matrix.
			int[] rowPivots = new int[rank + 1];
			int[] colPivots = new int[rank + 1];
			workMatrix = FormLU(workMatrix, ref rowPivots, ref colPivots, ref count);

			// ----- Get the product at each of the pivot points.
			result = 1;
			for (pivots = 0; pivots <= rank; pivots++)
			{
				result *= workMatrix[rowPivots[pivots], colPivots[pivots]];
			}

			// ----- Determine the sign of the result using LaPlace's formula.
			result = Math.Pow(-1, count) * result;
			return result;
		}

		public static double[] SimultEq(double[,] sourceEquations, double[] sourceRHS)
		{
			// ----- Use matrices to solve simultaneous equations.
			int rowsAndCols = 0;

			// ----- The matrix must be square and the array size must match.
			int rank = sourceEquations.GetUpperBound(0);
			if ((sourceEquations.GetUpperBound(1) != rank) || (sourceRHS.GetUpperBound(0) != rank))
			{
				throw new Exception("Size problem for simultaneous equations.");
			}

			// ----- Create some arrays for doing all of the work.
			double[,] coefficientMatrix = new double[rank + 1, rank + 1];
			double[] rightHandSide = new double[rank + 1];
			double[] solutions = new double[rank + 1];
			int[] rowPivots = new int[rank + 1];
			int[] colPivots = new int[rank + 1];

			// ----- Make copies of the original matrices so we don't
			//       mess them up.
			Array.Copy(sourceEquations, coefficientMatrix, sourceEquations.Length);
			Array.Copy(sourceRHS, rightHandSide, sourceRHS.Length);

			// ----- Use LU decomposition to form a triangular matrix.
			coefficientMatrix = FormLU(coefficientMatrix, ref rowPivots, ref colPivots, ref rowsAndCols);

			// ----- Find the unique solution for the upper-triangle.
			BackSolve(coefficientMatrix, rightHandSide, solutions, ref rowPivots, ref colPivots);

			// ----- Return the simultaneous equations result in an array.
			return solutions;
		}

		private static double[,] FormLU(double[,] sourceMatrix, ref int[] rowPivots, ref int[] colPivots, ref int rowsAndCols)
		{
			// ----- Perform an LU (lower and upper) decomposition of a matrix,
			//       a modified form of Gaussian elimination.
			int eachRow = 0;
			int eachCol = 0;
			int pivot = 0;
			int rowIndex = 0;
			int colIndex = 0;
			int bestRow = 0;
			int bestCol = 0;
			int rowToPivot = 0;
			int colToPivot = 0;
			double maxValue = 0;
			double testValue = 0;
			double oldMax = 0;
			const double Deps = 0.0000000000000001;

			// ----- Determine the size of the array.
			int rank = sourceMatrix.GetUpperBound(0);
			double[,] destMatrix = new double[rank + 1, rank + 1];
			double[] rowNorm = new double[rank + 1];
			rowPivots = new int[rank + 1];
			colPivots = new int[rank + 1];

			// ----- Make a copy of the array so we don't mess it up.
			Array.Copy(sourceMatrix, destMatrix, sourceMatrix.Length);

			// ----- Initialize row and column pivot arrays.
			for (eachRow = 0; eachRow <= rank; eachRow++)
			{
				rowPivots[eachRow] = eachRow;
				colPivots[eachRow] = eachRow;
				for (eachCol = 0; eachCol <= rank; eachCol++)
				{
					rowNorm[eachRow] += Math.Abs(destMatrix[eachRow, eachCol]);
				}
				if (rowNorm[eachRow] == 0)
				{
					throw new Exception("Cannot invert a singular matrix.");
				}
			}

			// ----- Use Gauss-Jordan elimination on the matrix rows.
			for (pivot = 0; pivot < rank; pivot++)
			{
				maxValue = 0;
				for (eachRow = pivot; eachRow <= rank; eachRow++)
				{
					rowIndex = rowPivots[eachRow];
					for (eachCol = pivot; eachCol <= rank; eachCol++)
					{
						colIndex = colPivots[eachCol];
						testValue = Math.Abs(destMatrix[rowIndex, colIndex]) / rowNorm[rowIndex];
						if (testValue > maxValue)
						{
							maxValue = testValue;
							bestRow = eachRow;
							bestCol = eachCol;
						}
					}
				}

				// ----- Detect a singular, or very nearly singular, matrix.
				if (maxValue == 0)
				{
					throw new Exception("Singular matrix used for LU.");
				}
				else if (pivot > 1)
				{
					if (maxValue < (Deps * oldMax))
					{
						throw new Exception("Non-invertible matrix used for LU.");
					}
				}
				oldMax = maxValue;

				// ----- Swap row pivot values for the best row.
				if (rowPivots[pivot] != rowPivots[bestRow])
				{
					rowsAndCols += 1;
					Swap(ref rowPivots[pivot], ref rowPivots[bestRow]);
				}

				// ----- Swap column pivot values for the best column.
				if (colPivots[pivot] != colPivots[bestCol])
				{
					rowsAndCols += 1;
					Swap(ref colPivots[pivot], ref colPivots[bestCol]);
				}

				// ----- Work with the current pivot points.
				rowToPivot = rowPivots[pivot];
				colToPivot = colPivots[pivot];

				// ----- Modify the remaining rows from the pivot points.
				for (eachRow = (pivot + 1); eachRow <= rank; eachRow++)
				{
					rowIndex = rowPivots[eachRow];
					destMatrix[rowIndex, colToPivot] = -destMatrix[rowIndex, colToPivot] / destMatrix[rowToPivot, colToPivot];
					for (eachCol = (pivot + 1); eachCol <= rank; eachCol++)
					{
						colIndex = colPivots[eachCol];
						destMatrix[rowIndex, colIndex] += destMatrix[rowIndex, colToPivot] * destMatrix[rowToPivot, colIndex];
					}
				}
			}

			// ----- Detect a non-invertible matrix.
			if (destMatrix[rowPivots[rank], colPivots[rank]] == 0)
			{
				throw new Exception("Non-invertible matrix used for LU.");
			}
			else if ((Math.Abs(destMatrix[rowPivots[rank], colPivots[rank]]) / rowNorm[rowPivots[rank]]) < (Deps * oldMax))
			{
				throw new Exception("Non-invertible matrix used for LU.");
			}

			// ----- Success. Return the LU triangular matrix.
			return destMatrix;
		}

		private static void Swap(ref int firstValue, ref int secondValue)
		{
			// ----- Reverse the values of two reference integers.
			int holdValue = firstValue;
			firstValue = secondValue;
			secondValue = holdValue;
		}

		private static void BackSolve(double[,] sourceMatrix, double[] rightHandSide, double[] solutions, ref int[] rowPivots, ref int[] colPivots)
		{
			// ----- Solve an upper-right-triangle matrix.
			int pivot = 0;
			int rowToPivot = 0;
			int colToPivot = 0;
			int eachRow = 0;
			int eachCol = 0;
			int rank = sourceMatrix.GetUpperBound(0);

			// ----- Work through all pivot points. This section builds
			//       the "B" in the AX=B formula.
			
            int tempVar = rank - 1;
			for (pivot = 0; pivot <= tempVar; pivot++)
			{
				colToPivot = colPivots[pivot];
				for (eachRow = (pivot + 1); eachRow <= rank; eachRow++)
				{
					rowToPivot = rowPivots[eachRow];
					rightHandSide[rowToPivot] += sourceMatrix[rowToPivot, colToPivot] * rightHandSide[rowPivots[pivot]];
				}
			}

			// ----- Now solve for each X using the general formula
			//       x(i) = (b(i) - summation(a(i,j)x(j)))/a(i,i)
			for (eachRow = rank; eachRow >= 0; eachRow--)
			{
				colToPivot = colPivots[eachRow];
				rowToPivot = rowPivots[eachRow];
				solutions[colToPivot] = rightHandSide[rowToPivot];
				for (eachCol = (eachRow + 1); eachCol <= rank; eachCol++)
				{
					solutions[colToPivot] -= sourceMatrix[rowToPivot, colPivots[eachCol]] * solutions[colPivots[eachCol]];
				}
				solutions[colToPivot] /= sourceMatrix[rowToPivot, colToPivot];
			}
		}
#endregion
		///########################################################################################################################
		///########################################################################################################################
	}

}