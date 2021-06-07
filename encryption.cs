using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingAdvisor
{
    class encryption
    {
        string LargeCharacters = "ABCDEFGHIJKLMNOPRSTUYZQXW";
        string LnewAllChars = "ABCDEFGHIJKLMNOPRSTUYZQXWABCDEFGHIJKLMNOPRSTUYZQXW";
        string Smallcharacters = "abcdefghijklmnoprstuyzqxw";
        string SnewAllChars = "abcdefghijklmnoprstuyzqxwabcdefghijklmnoprstuyzqxw";
        string numbers = "1234567890";
        string SmallAllChars = "abcdefghijklmnoprstuyzqxw1234567890";
        string LargeAllchars = "ABCDEFGHIJKLMNOPRSTUYZQXW1234567890";
        Random rnd;

        public string GenerateHashCode(string value)
        {
            int line = 0, rndmValue = 0, choose = 0;
            string hash = "", state = "", ch = "";
            for (int i = 0; i < value.Length; i++)
            {
                line = 0; rndmValue = 0; choose = 0;
                state = ""; ch = value[i].ToString();
                for (int y = 0; y < SmallAllChars.Length; y++)
                {
                    if (SmallAllChars[y].ToString() == ch)
                    {
                        line = y;
                        state = "small";
                        break;
                    }
                    else if (LargeAllchars[y].ToString() == ch)
                    {
                        line = y;
                        state = "LARGE";
                        break;
                    }
                }
                rndmValue = rnd.Next(1, 10); choose = line + rndmValue;
                if (state == "small")
                {
                    if (choose >= SmallAllChars.Length)
                        hash += SnewAllChars[choose].ToString() + rndmValue.ToString();
                    else
                        hash += SmallAllChars[choose].ToString() + rndmValue.ToString();
                }
                else if (state == "LARGE")
                {
                    string xLC = LargeCharacters + LargeCharacters;
                    if (choose >= LargeCharacters.Length)
                        hash += xLC[choose].ToString() + rndmValue.ToString();
                    else
                        hash += LargeCharacters[choose].ToString() + rndmValue.ToString();
                }
            }
            string newCh = "", newState = "", hash2 = "";
            int newLine = 0, newChoose = 0;
            for (int y = 0; y < hash.Length; y++)
            {
                newState = ""; newLine = 0; newChoose = 0; newCh = hash[y].ToString();
                for (int z = 0; z < SmallAllChars.Length; z++)
                {
                    if (SmallAllChars[z].ToString() == newCh)
                    {
                        newLine = z;
                        newState = "small";
                        break;
                    }
                    else if (LargeAllchars[z].ToString() == newCh)
                    {
                        newLine = z;
                        newState = "LARGE";
                        break;
                    }
                }
                newChoose = newLine + 2;
                if (newState == "small")
                {
                    if (newChoose >= SmallAllChars.Length)
                        hash2 += SnewAllChars[newChoose].ToString();
                    else
                        hash2 += SmallAllChars[newChoose].ToString();

                }
                else if (newState == "LARGE")
                {
                    string xLC = LargeCharacters + LargeCharacters;
                    if (newChoose >= LargeCharacters.Length)
                        hash2 += xLC[newChoose].ToString();
                    else
                        hash2 += LargeCharacters[newChoose].ToString();
                }
            }

            return hash2;
        }

        public string DeCrypt(string value)
        {
            string newCh = "", newState = "", decrypt2 = "";
            int newLine = 0, newChoose = 0;
            for (int y = 0; y < value.Length; y++)
            {
                newState = ""; newLine = 0; newChoose = 0; newCh = value[y].ToString();
                for (int z = 0; z < SmallAllChars.Length; z++)
                {
                    if (SmallAllChars[z].ToString() == newCh)
                    {
                        newLine = z;
                        newState = "small";
                        break;
                    }
                    else if (LargeAllchars[z].ToString() == newCh)
                    {
                        newLine = z;
                        newState = "LARGE";
                        break;
                    }
                }
                newChoose = 2;
                if (newState == "small")
                {
                    if (newLine - newChoose < 0)
                        decrypt2 += SnewAllChars[(SnewAllChars.Length + newLine) - newChoose].ToString();
                    else
                        decrypt2 += SmallAllChars[newLine - newChoose].ToString();
                }
                else if (newState == "LARGE")
                {
                    string xLC = LargeCharacters + LargeCharacters;
                    if (newLine - newChoose < 0)
                        decrypt2 += xLC[(xLC.Length + newLine) - newChoose].ToString();
                    else
                        decrypt2 += LargeCharacters[newLine - newChoose].ToString();
                }
            }
            value = decrypt2;
            string twoChar = "", decrypt = "", state = "";
            int slipValue = 0, line = 0;
            for (int i = 0; i < value.Length; i += 2)
            {
                line = 0;
                twoChar = value[i].ToString() + value[i + 1].ToString();
                slipValue = Convert.ToInt32(twoChar[1].ToString());
                for (int x = 0; x < SmallAllChars.Length; x++)
                {
                    if (twoChar[0] == SmallAllChars[x])
                    {
                        state = "small";
                        line = x;
                        break;
                    }
                    else if (twoChar[0] == LargeAllchars[x])
                    {
                        state = "LARGE";
                        line = x;
                        break;
                    }
                }
                if (state == "small")
                {
                    if (line - slipValue < 0)
                        decrypt += SnewAllChars[(SmallAllChars.Length + line) - slipValue].ToString();
                    else
                        decrypt += SmallAllChars[line - slipValue].ToString();
                }
                else if (state == "LARGE")
                {
                    string xLC = LargeCharacters + LargeCharacters;

                    if (line - slipValue < 0)
                        decrypt += xLC[(LargeCharacters.Length + line) - slipValue].ToString();
                    else
                        decrypt += LargeCharacters[line - slipValue].ToString();
                }
            }
            return decrypt;
        }
    }
}
