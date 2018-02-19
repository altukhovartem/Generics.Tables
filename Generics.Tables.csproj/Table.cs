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
        public List<T1> Rows { get; set; }
        public List<T2> Columns { get; set; }
        
        public T3 Value { get; set; }

        public OpenIndexator Open { get; set; }


        public Table()
        {
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
                    if (table.Rows.Count == 0)
                        table.Rows.Add(t1);
                    if (table.Rows.Count == 0)
                        table.Columns.Add(t2);
                    value = table.dictionary.Add(Tuple.Create(t1, t2));
                }
            }

        }
    }
}
