using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 1. var table = new Table< int, string, double>(); - строки ты индексуешь числами, столбцы стрингами
/// 2. table.Open[1, "2"] = 3 - то положить в конкретную ячейку значение, в первую строку в колонку с именем 2 положить тройку
/// 3. table.Existed["1", 1] - прочитать из существующей ячейки, тот должно падать потому что колонку создали, а строку нет, получается и ячейки нет
/// 4. у тебя есть два вида индексаторов
/// создающий и несоздающий
/// обращение через создающий, будь то чтение или запись всегда должен создавать в случае отсутствия все необходимые компоненты ячейки
/// то есть учитывать 4 случая(edited)
/// когда и строка и столбец к которым производится доступ уже созданы
/// когда строка создана, а столбец нет
/// когда столбец создан а строка нет
/// и когда ничего еще не создано
/// ну и соответственно должен досоздать
/// что не хватает
/// несоздающий очевидно ничего не создает
/// а бросает исключение если чего то не хватает при обращении
/// </summary>

namespace Generics.Tables
{
    public class Table<T1, T2, T3>
    {
        private Dictionary<Tuple<T1, T2>, T3> dictionary = new Dictionary<Tuple<T1, T2>, T3>();
        private List<T1> rows = new List<T1>();
        private List<T2> columns = new List<T2>();

        public IEnumerable<T1> Rows { get => rows;}
        public IEnumerable<T2> Columns { get => columns;}
        public OpenIndexator Open { get; }
        public ExistedIndexator Existed { get; }

        public void AddRow(T1 t1)
        {
            if (!rows.Contains(t1))
                rows.Add(t1);
        }

        public void AddColumn(T2 t2)
        {
            if (!columns.Contains(t2))
                columns.Add(t2);
        }

        public Table()
        {
            Open = new OpenIndexator(this);
            Existed = new ExistedIndexator(this);
        }

        public class OpenIndexator
        {
            private Table<T1, T2, T3> table;
     
            public OpenIndexator(Table<T1, T2, T3> table)
            {
                this.table = table;
            }

            public T3 this[T1 t1, T2 t2]
            {
                set
                {
                    Tuple<T1, T2> Key = Tuple.Create(t1, t2); 
                    if (table.dictionary.ContainsKey(Key))
                    {
                        table.dictionary[Key] = value;
                    }
                    else
                    {
                        if (!table.Rows.Contains(t1))
                            table.AddRow(t1);
                        if (!table.Columns.Contains(t2))
                            table.AddColumn(t2);
                        table.dictionary.Add(Key, value);
                    }
                }
                get
                {
                    Tuple<T1, T2> Key = Tuple.Create(t1, t2);
                    if (table.dictionary.ContainsKey(Key))
                        return table.dictionary[Key];
                    else
                        return default(T3);
                }
            }
        }

        public class ExistedIndexator
        {
            private Table<T1, T2, T3> table;
            public ExistedIndexator(Table<T1,T2,T3> table)
            {
                this.table = table;
            }
            public T3 this[T1 t1, T2 t2]
            {
                set
                {
                    Tuple<T1, T2> Key = Tuple.Create(t1, t2);
                    if (!table.Rows.Contains(t1) || !table.Columns.Contains(t2))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        table.dictionary[Key] = value;
                    }
                }
                get
                {
                    Tuple<T1, T2> Key = Tuple.Create(t1, t2);
                    if (table.rows.Contains(t1) && table.columns.Contains(t2))
                    {
                        if (table.dictionary.ContainsKey(Key))
                            return table.dictionary[Key];
                        else
                            return default(T3);
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
        }
    }
}
