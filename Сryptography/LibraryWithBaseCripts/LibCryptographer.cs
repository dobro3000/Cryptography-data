using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LibraryWithBaseCripts
{
    public class LibCryptographer
    {
        public System.IO.FileStream stream { get; private set; }

        public void Cryptographer(string filename, string word, string typeCript, bool decOrUn)
        {
            switch (typeCript.ToLower())
            {
                case "permutation": Permutation(filename, word, decOrUn); break;
                case "monoalphabetic": Monoalphabetic(filename, word, decOrUn); break;
                case "polyalphabetic": Polyalphabetic(filename, word, decOrUn); break;
                case "summirovanii": Summirovanii(filename, word, decOrUn); break;
            }
        }

        private void Permutation(string filename, string word, bool decOrUn)
        {
            //Присваиваем каждой букве номер столбца.
            Dictionary<int, char> key = new Dictionary<int, char>();
            //Колличество колонок в массиве.
            int lengthColon = word.Length;
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //Массив данных из файла.
            byte[] buff = new byte[stream.Length];
            stream.Read(buff, 0, buff.Length);
            stream.Close();
            //Колличество строк в массиве.
            int lengthLine = (int)Math.Ceiling((double)buff.Length / (double)word.Length);
            //Счетчик.
            int count = 0;
            //Определяем размерность массива.
            byte[,] mas = new byte[lengthColon, lengthLine];
            byte[,] newMasForKey = new byte[lengthColon, lengthLine];

            if (decOrUn)
            {
                //Заполняем массив.
                for (int j = 0; j < lengthLine; j++)
                {
                    for (int i = 0; i < lengthColon; i++)
                    {
                        if (count < buff.Length)
                            mas[i, j] = buff[count++];
                        else break;
                    }
                }

                for (int i = 0; i < lengthColon; i++)
                    key.Add(i, word[i]);

                count = 0;
                foreach (var k in key.OrderBy(k => k.Value))
                {
                    for (int j = 0; j < lengthLine; j++)
                        newMasForKey[count, j] = mas[k.Key, j];
                    count++;
                }

                count = 0;
                byte[] str = new byte[mas.Length];
                for (int i = 0; i < lengthColon; i++)
                    for (int j = 0; j < lengthLine; j++)
                    {
                        str[count++] = newMasForKey[i, j];
                    }
                File.WriteAllBytes(filename, str);
                MessageBox.Show("It's all! Check IT!");
            }
            else
            {
                for (int i = 0; i < lengthColon; i++)
                {
                    for (int j = 0; j < lengthLine; j++)
                    {
                        if (count < buff.Length)
                            mas[i, j] = buff[count++];
                        else break;
                    }
                }
                count = 0;
                foreach (var s in word.OrderBy(s => (byte)s))
                    key.Add(count++, s);

                count = 0;
                foreach (char w in word)
                {
                    for (int j = 0; j < lengthLine; j++)
                        newMasForKey[count, j] = mas[key.FirstOrDefault(x => x.Value == w).Key, j];

                    key.Remove(key.FirstOrDefault(x => x.Value == w).Key);
                    count++;
                }

                count = 0;
                byte[] str = new byte[mas.Length];
                for (int j = 0; j < lengthLine; j++)
                    for (int i = 0; i < lengthColon; i++)
                    {
                        str[count++] = newMasForKey[i, j];
                    }
                File.WriteAllBytes(filename, str);
                MessageBox.Show("It's all! Check IT!");
            }
        }

        private void Monoalphabetic(string filename, string word, bool decOrUn)
        {
            byte shift = Convert.ToByte(word);

            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] buff = new byte[stream.Length];
            stream.Read(buff, 0, buff.Length);
            stream.Close();

            byte[] newMas = new byte[buff.Length];

            if (decOrUn)
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    newMas[i] = (byte)((buff[i] + shift) % 256);
                }
            }
            else
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    newMas[i] = (byte)((buff[i] - shift) % 256);
                }
            }

            File.WriteAllBytes(filename, newMas);
            MessageBox.Show("It's all! Check IT!");

        }

        private void Polyalphabetic(string filename, string word, bool decOrUn)
        {
            int count = 0;

            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] buff = new byte[stream.Length];
            stream.Read(buff, 0, buff.Length);
            stream.Close();

            byte[] newMas = new byte[buff.Length];

            if (decOrUn)
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    if (count == word.Length)
                        count = 0;
                    //newMas[i] = (byte)(buff[i] + (byte)Math.Abs((byte)word[count++] % 100));
                    newMas[i] = (byte)((byte)(buff[i] + (byte)word[count++]) % 256);
                }
            }
            else
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    if (count == word.Length)
                        count = 0;
                    //newMas[i] = (byte)(buff[i] - (byte)Math.Abs((byte)word[count++] % 100));
                    newMas[i] = (byte)((byte)(256 + buff[i] - (byte)word[count++]) % 256);
                }
            }

            File.WriteAllBytes(filename, newMas);
            MessageBox.Show("It's all! Check IT!");

        }

        private void Summirovanii(string filename, string word, bool decOrUn)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] buff = new byte[stream.Length];
            stream.Read(buff, 0, buff.Length);
            stream.Close();

            int count = 0;

            List<long> kye = new List<long>();
            List<long> bytes = new List<long>();
            byte[] newMas = new byte[buff.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                List<long> temp = new List<long>();
                bytes = GetBinaryCode(buff[i]);
                kye = GetBinaryCode((byte)word[count++]);
                for(int j = 0; j < bytes.Count; j++)
                {
                   temp.Add((bytes[j] + kye[j])%2);
                }
                if(count > 3)
                {
                    count = 0;
                }
                newMas[i] = GetCharCode(temp);
                temp.Clear();
            }

            File.WriteAllBytes(filename, newMas);
            MessageBox.Show("It's all! Check IT!");
        }

        private List<long> GetBinaryCode(byte b)
        {
            int ost = b;
            Stack<long> newB = new Stack<long>();
           // List<long> newB = new List<long>();
            List<long> getB = new List<long>();
            while (ost > 1)
            {
                long t = ost % 2;
                newB.Push(t);
                ost = (int)(ost / 2);
            }

            newB.Push(ost);

            while(newB.Count != 64)
            {
                newB.Push(0);
            }

                foreach (long l in newB)
                getB.Add(l);

            return getB;
        }

        private byte GetCharCode(List<long> b)
        {
            byte sum = 0;
            int count = 0;
            for(int i = b.Count - 1; i >= 0; i--)
            {
                sum += (byte)(b[i] * Math.Pow(2,count++));
            }
            return sum;
        }



    }
}
