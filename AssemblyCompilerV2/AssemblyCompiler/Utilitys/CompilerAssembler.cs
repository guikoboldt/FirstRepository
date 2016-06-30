using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCompiler.Utilitys
{
    public class CompilerAssembler
    { 
        public string assembly { get; set; }
        public string languageBinary { get; set; }
        public string languageHexa { get; set; }
        public string controlSingal { get; set; }
        private readonly IDictionary <string, string> operators = new Dictionary<string, string>
        {
            { "add", "100000" },
            { "addi", "001000" },
            { "sub", "100010" },
            { "and", "100100" },
            { "andi", "001100" },
            { "or", "100101" },
            { "ori" , "001101" },
            { "beq" , "000100" },
            { "slt" , "101010" },
            { "slti" , "001010" },
            { "j" , "000010" },            
            { "lw", "100011" },
            { "sw", "101011" },
        };
        private readonly IDictionary<string, string> registrators = new Dictionary<string, string>
        {
            { "$0", "00000" },
            { "$at", "00001" },
            { "$v0", "00010" },
            { "$v1", "00011" },
            { "$a0", "00100" },
            { "$a1", "00101" },
            { "$a2" , "00110" },
            { "$a3" , "00111" },
            { "$t0" , "01000" },
            { "$t1" , "01001" },
            { "$t2" , "01010" },
            { "$t3" , "01011" },
            { "$t4" , "01100" },
            { "$t5", "01101" },
            { "$t6", "01110" },
            { "$t7", "01111" },
            { "$s0", "10000" },
            { "$s1", "10001" },
            { "$s2", "10010" },
            { "$s3", "10011" },
            { "$s4", "10100" },
            { "$s5", "10101" },
            { "$s6", "10110" },
            { "$s7", "10111" },
            { "$t8", "11000" },
            { "$t9", "11001" },
            { "$k0", "11010" },
            { "$k1", "11011" },
            { "$gp", "11100" },
            { "$sp", "11101" },
            { "$fp", "11110" },
            { "$s8", "11110" },
            { "$ra", "11111" },
        };
        private readonly IDictionary<string, string> memoryAddress = new Dictionary<string, string>
        {
            { "a", "00000000000000000000000100" },
            { "A", "00000000000000000000001000" },
            { "b", "00000000000000000000001100" },
            { "B", "00000000000000000000010000" },
            { "c", "00000000000000000000010100" },
            { "C", "00000000000000000000011000" },
            { "d", "00000000000000000000011100" },
            { "D", "00000000000000000000100000" },
            { "e", "00000000000000000000100100" },
            { "E", "00000000000000000000101000" },
            { "f", "00000000000000000000101100" },
            { "F", "00000000000000000000110000" },
            { "g", "00000000000000000000110100" },
            { "G", "00000000000000000000111000" },
            { "h", "00000000000000000000111100" },
            { "H", "00000000000000000001000000" },
        };

        public string DetermineType (string codeLine)
            //determina o tipo das instruções
        {
            var type = "";
            
            if (operators.ContainsKey(codeLine))
            {
                type = (from a in operators
                             where a.Key.Equals(codeLine)
                             select new { Key = a.Key, Value = a.Value }).FirstOrDefault().Value;
            }
            else
            {
                type = "0";
            }
            return type;
        } 

        public string immediateToBin (string immediate)
            //converte as constantes do tipo i para binário
        {
            short v = 0;
            string ret = "";
            string clean = immediate.Trim();
            bool test = true;
            try
            {
                v = Convert.ToInt16(clean, 10);
            }
            catch (Exception e)
            {
                ret = "";
                test = false;
            }
            if (test)
            {                
                ret = Convert.ToString(v, 2);
                int l = ret.Length;
                while (l < 16)
                {
                    ret = "0" + ret;
                    l++;
                }
            }            
            return ret;
        }
        public string registerToBin (string register)
            //converte os registradores para binário
        {
            var registerCode = "";
            string registerC = register.Trim();
            if (registrators.ContainsKey(registerC))
            {
                registerCode = (from a in registrators
                        where a.Key.Equals(registerC)
                        select new { Key = a.Key, Value = a.Value }).FirstOrDefault().Value;
            }
            else
            {
                registerCode = "0";
            }
            return registerCode;
        }
        public string getAddressForJBin (string address)
            //banco de dados de endereços virtual, converte para binário
        {
            //var ret = "0";
            var result = "";
            var cleanedAddress = address.Trim();
            if (memoryAddress.ContainsKey(cleanedAddress))
            {
                result = (from a in memoryAddress
                          where a.Key.Equals(cleanedAddress)
                                select new { Key = a.Key, Value = a.Value }).FirstOrDefault().Value;
            }
            else
            {
                result = "0";
            }
          
            return result;
        }
        public string convertToHex (string binCode)
            //converte a string de código binário para código hex
        {
            string code = "";
            int cValue = 0;
            cValue = Convert.ToInt32(binCode, 2);
            code = code + Convert.ToString(cValue, 16);
            int l = code.Length;
            while (l < 8)
            {
                code = "0" + code;
                l++;
            }
            code = "0x" + code;

            return code;
        }
        public int getTypeID (string commandLine)
            //usado pelo lineToBin para determinar o tipo de instrução
        {
            int typeID = 0;
            if (commandLine.StartsWith("add "))
            {
                typeID = 1;
            }
            else if (commandLine.StartsWith("addi "))
            {
                typeID = 2;
            }
            else if (commandLine.StartsWith("sub "))
            {
                typeID = 3;
            }
            else if (commandLine.StartsWith("and "))
            {
                typeID = 4;
            }
            else if (commandLine.StartsWith("andi "))
            {
                typeID = 5;
            }
            else if (commandLine.StartsWith("or "))
            {
                typeID = 6;
            }
            else if (commandLine.StartsWith("ori "))
            {
                typeID = 7;
            }
            else if (commandLine.StartsWith("beq "))
            {
                typeID = 8;
            }
            else if (commandLine.StartsWith("slt "))
            {
                typeID = 9;
            }
            else if (commandLine.StartsWith("slti "))
            {
                typeID = 10;
            }
            else if (commandLine.StartsWith("j "))
            {
                typeID = 11;
            }
            else if (commandLine.StartsWith("lw "))
            {
                typeID = 12;
            }
            else if (commandLine.StartsWith("sw "))
            {
                typeID = 13;
            }
            else
            {
                typeID = 0;
            }
            return typeID;
        }
        public string lineToBin (string codeLine, int line)
            //converte o código de uma linha inteira para binário
        {
            string binCode = "";
            int typeID = getTypeID(codeLine);
            string cleanCode = codeLine.Replace("(", " ");
            cleanCode = cleanCode.Replace(")", "");
            cleanCode = cleanCode.Replace(",", "");
            string[] splitCode = cleanCode.Split(' ');            
            switch (typeID)
            {
                case 1://add
                    {
                        binCode = "000000";
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + "00000";
                        binCode = binCode + DetermineType(splitCode[0]);
                        break;
                    }
                case 2://addi
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[3]);
                        break;
                    }
                case 3://sub
                    {                        
                        binCode = "000000";
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + "00000";
                        binCode = binCode + DetermineType(splitCode[0]);
                        break;
                    }
                case 4://and
                    {
                        binCode = "000000";
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + "00000";
                        binCode = binCode + DetermineType(splitCode[0]);
                        break;
                    }
                case 5://andi
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[3]);
                        break;
                    }
                case 6://or
                    {
                        binCode = "000000";
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + "00000";
                        binCode = binCode + DetermineType(splitCode[0]);
                        break;
                    }
                case 7://ori
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[3]);
                        break;
                    }
                case 8://beq
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + getLabel(splitCode[3], line);
                        break;
                    }
                case 9://slt
                    {
                        binCode = "000000";
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + "00000";
                        binCode = binCode + DetermineType(splitCode[0]);
                        break;
                    }
                case 10://slti
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[2]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[3]);
                        break;
                    }
                case 11://j
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + getAddressForJBin(splitCode[1]);
                        break;
                    }
                case 12://lw
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[2]);
                        break;
                    }
                case 13://sw
                    {
                        binCode = DetermineType(splitCode[0]);
                        binCode = binCode + registerToBin(splitCode[3]);
                        binCode = binCode + registerToBin(splitCode[1]);
                        binCode = binCode + immediateToBin(splitCode[2]);                        
                        break;
                    }
                default:
                    {
                        binCode = "0";
                        break;
                    }
            }                                    
            return binCode;
        }
        public string getLabel (string label, int line)
            //obtem o label a partir de um numero e da linha do beq
        {
            short l = 0;
            short immed = 0;
            string code = "";
            bool test = true;
            string clean = label.Trim();
            try
            {
                l = Convert.ToInt16(clean, 10);
            }
            catch (Exception e)
            {
                code = "";
                test = false;
            }
            if (test)
            {
                l = (short)(l - 1);
                immed = (short)(l - line);
                code = Convert.ToString(immed, 2);
            }
            return code;
        }
        public string getControlSignal (string codeLine)
            //obtem o sinal de controle da ula equivalente ao da instrução
        {
            string signal = "XXX";
            int command = getTypeID(codeLine);
            switch (command)
            {
                case 1:
                    signal = "110";
                    break;
                case 2:
                    signal = "110";
                    break;
                case 3:
                    signal = "111";
                    break;
                case 4:
                    signal = "000";
                    break;
                case 5:
                    signal = "000";
                    break;
                case 6:
                    signal = "001";
                    break;
                case 7:
                    signal = "001";
                    break;
                case 9:
                    signal = "101";
                    break;
                case 10:
                    signal = "101";
                    break;
                default:
                    signal = "XXX";
                    break;
            }
            return signal;
        }
    }
}
