using System;
using System.Collections;
using System.Text;

class Matrix<T> : ICloneable, IEnumerable, IComparable where T : IComparable, new()
{
    private T[,] matrix;
    public Matrix()
    {
        matrix = new T[0,0];
    }
    public Matrix(T[,] mat)
    {
        if (mat.GetUpperBound(0) + 1 != mat.Length / (mat.GetUpperBound(0) + 1))
            throw new MyException("Matrix is not square");
        matrix = mat;
    }

    public Matrix(int size)
    {
        if ( size < 0 )
            throw new MyException("Bad size");
        matrix = new T[size, size];
    }

    public Matrix<T> Trans()
    {
        if (matrix.GetUpperBound(0) == -1)
            return new Matrix<T>();
        var res = new Matrix<T>(matrix.GetUpperBound(0)+1);
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
            for (int j = 0; j < matrix.GetUpperBound(0)+1; j++)
                res.matrix[j, i] = matrix[i, j];
        return res;
    }

    public T Determ()
    {
        if (matrix.GetUpperBound(0) == -1)
            throw new MyException("Matrix is not exists");
        if (matrix.GetUpperBound(0) == 0)
        {
            return matrix[0, 0];
        }
        if (matrix.GetUpperBound(0) == 1)
        {
            return (dynamic)matrix[0, 0] * matrix[1, 1] - (dynamic)matrix[0, 1] * matrix[1, 0];
        }
        var res = new T();
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
        {
            if (i % 2 == 0)
            {
                res += (dynamic) matrix[0, i] * SubMatrix(matrix, 0, i);
            }
            else
            {
                res -= (dynamic) matrix[0, i] * SubMatrix(matrix, 0, i);
            }
        }
        return res;
    }

    private T SubMatrix(T[,] mat, int sub_i, int sub_j)
    {
        var subMat = new T[mat.GetUpperBound(0), mat.GetUpperBound(0)];
        for (int i = 0, icounter = 0; i < mat.GetUpperBound(0)+1; i++, icounter++)
        {
            if (i != sub_i)
            {
                for (int j = 0, jcounter = 0; j < mat.GetUpperBound(0)+1; j++, jcounter++)
                {
                    if (j != sub_j)
                    {
                        subMat[icounter, jcounter] = mat[i, j];
                    }
                    else
                    {
                        jcounter--;
                    }
                }
            }
            else
            {
                icounter--;
            }
        }
        if (mat.GetUpperBound(0) == 1)
        {
            return subMat[0, 0];
        }
        if (mat.GetUpperBound(0) == 2)
        {
            return (dynamic)subMat[0, 0] * subMat[1, 1] - (dynamic)subMat[0, 1] * subMat[1, 0];
        }
        T res = new T();
        for (int i = 0; i < mat.GetUpperBound(0); i++)
        {
            if (i% 2 == 0)
            {
                res += (dynamic)subMat[0, i] * SubMatrix(subMat, 0, i);
            }
            else
            {
                res -= (dynamic)subMat[0, i] * SubMatrix(subMat, 0, i);
            }
        }
        return res;
    }

    public Matrix<T> Reverse()
    {
        if (matrix.GetUpperBound(0) == -1)
            return this;
        else if (matrix.GetUpperBound(0) == 0)
            return new Matrix<T>(new T[1, 1] { { Math.Pow((dynamic)matrix[0, 0], -1) } });
        var res = new Matrix<T>(matrix.GetUpperBound(0)+1);
        var mat = new Matrix<T>(matrix.GetUpperBound(0));
        var det = Determ();
        if (det.Equals(0))
            throw new MyException("Devision by zero");
        for (var i = 0; i < matrix.GetUpperBound(0)+1; i++)
            for (var j = 0; j < matrix.GetUpperBound(0)+1; j++)
            {
                var j_el = 0;
                for (var k = 0; k < matrix.GetUpperBound(0); k++)
                {
                    if (k == i)
                        j_el = 1;
                    var i_el = 0;
                    for (int l = 0; l < matrix.GetUpperBound(0); l++)
                    {
                        if (l == j)
                            i_el = 1;
                        mat.matrix[k, l] = matrix[k + j_el, l + i_el];
                    }
                }
                if ((i + j) % 2 == 0)
                    res.matrix[i, j] = (dynamic)mat.Determ();
                else
                    res.matrix[i, j] = (dynamic)mat.Determ() * (-1);
            }
        res = res.Trans();
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
            for (int j = 0; j < matrix.GetUpperBound(0)+1; j++)
                res.matrix[i, j] = (dynamic)res.matrix[i, j] / det;
        return res;
    }
    public static bool operator ==(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) != mat2.matrix.GetUpperBound(0))
            return false;
        for (int i = 0; i < mat1.matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < mat1.matrix.GetUpperBound(0) + 1; j++)
            {
                if (mat1.matrix[i, j] != (dynamic)mat2.matrix[i, j])
                    return false;
            }
        }
        return true;
    }

    public static bool operator !=(Matrix<T> mat1, Matrix<T> mat2)
    {
         return mat1 == mat2 ? true : false;
    }

    public static Matrix<T> operator *(Matrix<T> mat, T num)
    {
        if (mat.matrix.GetUpperBound(0) == -1)
            return mat;
        var obj = new Matrix<T>(mat.matrix.GetUpperBound(0)+1);
        for (int i = 0; i < mat.matrix.GetUpperBound(0) + 1; i++)
        {
            for (int j = 0; j < mat.matrix.GetUpperBound(0) + 1; j++)
            {
                obj.matrix[i, j] = (dynamic)mat.matrix[i, j] * num;
            }
        }
        return obj;
    }

    public static Matrix<T> operator +(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) == -1)
            return mat2;
        else if (mat2.matrix.GetUpperBound(0) == -1)
            return mat1;
        else if (mat1.matrix.GetUpperBound(0) != mat2.matrix.GetUpperBound(0))
            throw new MyException("Matrix sizes are not equal");
        var res = new Matrix<T>(mat1.matrix.GetUpperBound(0)+1);
        for (int i = 0; i < mat1.matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < mat1.matrix.GetUpperBound(0)+1; j++)
            {
                res.matrix[i, j] = (dynamic)mat1.matrix[i, j] + mat2.matrix[i, j];
            }
        }
        return res;
    }

    public static Matrix<T> operator -(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) == -1)
            return (dynamic)mat2 * (-1);
        else if (mat2.matrix.GetUpperBound(0) == -1)
            return mat1;
        else if (mat1.matrix.GetUpperBound(0) != mat2.matrix.GetUpperBound(0))
            throw new MyException("Matrix sizes are not equal");
        var res = new Matrix<T>(mat1.matrix.GetUpperBound(0)+1);
        for (int i = 0; i < mat1.matrix.GetUpperBound(0) + 1; i++)
        {
            for (int j = 0; j < mat1.matrix.GetUpperBound(0) + 1; j++)
            {
                res.matrix[i, j] = (dynamic)mat1.matrix[i, j] - mat2.matrix[i, j];
            }
        }
        return res;
    }

    public static Matrix<T> operator *(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) != mat2.matrix.GetUpperBound(0))
            throw new MyException("Matrix sizes are not equal");
        var res = new Matrix<T>(mat1.matrix.GetUpperBound(0)+1);
        for (int i = 0; i < mat1.matrix.GetUpperBound(0) + 1; i++)
        {
            for (int j = 0; j < mat1.matrix.GetUpperBound(0) + 1; j++)
            {
                res.matrix[i, j] = new T();
                for (int k = 0; k < mat1.matrix.GetUpperBound(0) + 1; k++)
                    res.matrix[i, j] = (dynamic)mat1.matrix[i, k] * mat2.matrix[k, j]+ res.matrix[i, j];
            }
        }
        return res;
    }

    public static Matrix<T> operator /(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) == -1 && mat2.matrix.GetUpperBound(0) == -1)
            return mat1;
        if (mat1.matrix.GetUpperBound(0) != mat2.matrix.GetUpperBound(0))
            throw new MyException("Matrix sizes are not equal");
        return (mat1 * mat2.Reverse());
    }

    public static bool operator >(Matrix<T> mat1, Matrix<T> mat2)
    {
        if (mat1.matrix.GetUpperBound(0) == -1 && mat1.matrix.GetUpperBound(0) == -1)
            return true;
        else if (mat1.matrix.GetUpperBound(0) == -1)
            return false;
        else if (mat2.matrix.GetUpperBound(0) == -1)
            return true;
        else
            return (dynamic) mat1.Determ() > mat2.Determ();
    }

    public static bool operator <(Matrix<T> mat1, Matrix<T> mat2)
    {
        return mat2>mat1;
    }

    public override string ToString()
    {
        if (matrix.GetUpperBound(0)+1 == 0)
            return String.Empty;
        var sb = new StringBuilder();
        sb.Append("(( ");//temp
        for (int i = 0; i < matrix.GetUpperBound(0)+1; i++)
        {
            for (int j = 0; j < matrix.GetUpperBound(0)+1; j++)
                sb.Append($"{this.matrix[i, j]} ");
            if (i != matrix.GetUpperBound(0))
                sb.Append("), (");
                //sb.AppendLine();
        }
        sb.Append("))");//temp
        return sb.ToString();
    }
    public object Clone()
    {
        if (matrix.GetUpperBound(0) == -1)
            return new Matrix<T> { matrix = new Matrix<T>(matrix.GetUpperBound(0) + 1).matrix };
        var res = new Matrix<T>(matrix.GetUpperBound(0) + 1);
        for (int i = 0; i < matrix.GetUpperBound(0) + 1; i++)
            for (int j = 0; j < matrix.GetUpperBound(0) + 1; j++)
                res.matrix[i, j] = this.matrix[i, j];
        return new Matrix<T>{ matrix = res.matrix };
    }
    public IEnumerator GetEnumerator()
    {
        return matrix.GetEnumerator();
    }
    public int CompareTo(Matrix<T> mat)
    {
        var det1 = this.Determ();
        var det2 = mat.Determ();
        if ((dynamic)det1 > det2)
            return 1;
        else if ((dynamic)det1 < det2)
            return -1;
        else
            return 0;
    }

    public int CompareTo(object obj)
    {
        if (!(obj is Matrix<T>))
            return 0;
        return CompareTo((Matrix<T>)obj);
    }
}
