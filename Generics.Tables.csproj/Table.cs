using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 1. var table = new Table<int, string, double>(); - строки ты индексуешь числами, столбцы стрингами
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
        Dictionary<Tuple<T1,T2>, T3> dictionary = new Dictionary<Tuple<T1, T2>, T3>(); 
        public IEnumerable<T1> Rows { get; set; }
        public IEnumerable<T2> Columns { get; set; }
        public OpenIndexator Open { get; set; }

        public void AddRow(T1 t1)
        {
            ((List<T1>)Rows).Add(t1);
        }

        public void AddColumn(T2 t2)
        {
            ((List<T2>)Columns).Add(t2);
        }

        public Table()
        {
            Rows = new List<T1>();
            Columns = new List<T2>();
            Open = new OpenIndexator(this);
        }

        public class OpenIndexator
        {
            Table<T1, T2, T3> table;
     
            public OpenIndexator(Table<T1, T2, T3> table)
            {
                this.table = table;
            }

            public T3 this[T1 t1, T2 t2]
            {
                set
                {
                    Tuple<T1, T2> Key = Tuple.Create(t1, t2); 
                    if (!table.dictionary.ContainsKey(Key))
                    {
                        if (!table.Rows.Contains(t1))
                            table.AddRow(t1);
                        if (!table.Columns.Contains(t2))
                            table.AddColumn(t2);
                        table.dictionary.Add(Key, value);
                    }
                    else
                    {
                        table.dictionary[Key] = value;
                    }
                }
                get
                {
                    return table.dictionary[Tuple.Create(t1, t2)];
                }
            }

        }
    }
}
