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
            { "jr" , "001000" },
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

            //int type = 0;
            //if (codeLine.StartsWith("add "))
            //{
            //    type = 1;
            //}
            //else if (codeLine.StartsWith("addi "))
            //{
            //    type = 2;            
            //}
            //else if (codeLine.StartsWith("sub "))
            //{
            //    type = 3;
            //}
            //else if (codeLine.StartsWith("and "))
            //{
            //    type = 4;
            //}
            //else if (codeLine.StartsWith("andi "))
            //{
            //    type = 5;
            //}
            //else if (codeLine.StartsWith("or "))
            //{
            //    type = 6;
            //}
            //else if (codeLine.StartsWith("ori "))
            //{
            //    type = 7;
            //}
            //else if (codeLine.StartsWith("beq "))
            //{
            //    type = 8;
            //}
            //else if (codeLine.StartsWith("slt "))
            //{
            //    type = 9;
            //}
            //else if (codeLine.StartsWith("slti "))
            //{
            //    type = 10;
            //}
            //else if (codeLine.StartsWith("j "))
            //{
            //    type = 11;
            //}
            //else if (codeLine.StartsWith("jr "))
            //{
            //    type = 12;
            //}
            //else if (codeLine.StartsWith("lw "))
            //{
            //    type = 13;
            //}
            //else if (codeLine.StartsWith("sw "))
            //{
            //    type = 14;
            //}
            return type;
        } 
       
        //public string getOpCode (int type)
        //    //determina o opcode das instruções
        //{
        //    string ret = "";
        //    switch (type)
        //    {
        //        case 1:
        //            ret = "100000";                    
        //        case 2:
        //            ret = "001000";                    
        //        case 3:
        //            ret = "100010";                    
        //        case 4:
        //            ret = "100100";                    
        //        case 5:
        //            ret = "001100";                    
        //        case 6:
        //            ret = "100101";                    
        //        case 7:
        //            ret = "001101";                    
        //        case 8:
        //            ret = "000100";                    
        //        case 9:
        //            ret = "101010";                    
        //        case 10:
        //            ret = "001010";                    
        //        case 11:
        //            ret = "000010";                    
        //        case 12:
        //            ret = "001000";                    
        //        case 13:
        //            ret = "100011";                    
        //        case 14:
        //            ret = "101011";                    
        //        default:
        //            ret = "0";  
        //    }
        //    return ret;            
        //}
        public string immediateToBin (string immediate)
            //converte as constantes do tipo i para binário
        {
            short v = 0;
            string ret = "";
            v = Convert.ToInt16(immediate, 2);
            ret = Convert.ToString(v, 2);
            return ret;
        }
        public string registerToBin (string register)
            //converte os registradores para binário
        {
            var registerCode = "";

            if (registrators.ContainsKey(register))
            {
                registerCode = (from a in registrators
                        where a.Key.Equals(register)
                        select new { Key = a.Key, Value = a.Value }).FirstOrDefault().Value;
            }
            else
            {
                registerCode = "0";
            }
            return registerCode;
            //string ret = "0";
            //if (register.Equals("$0"))
            //    ret = "00000";
            //else if (register.Equals("$at"))
            //    ret = "00001";
            //else if (register.Equals("$v0"))
            //    ret = "00010";
            //else if (register.Equals("$v1"))
            //    ret = "00011";
            //else if (register.Equals("$a0"))
            //    ret = "00100";
            //else if (register.Equals("$a1"))
            //    ret = "00101";
            //else if (register.Equals("$a2"))
            //    ret = "00110";
            //else if (register.Equals("$a3"))
            //    ret = "00111";
            //else if (register.Equals("$t0"))
            //    ret = "01000";
            //else if (register.Equals("$t1"))
            //    ret = "01001";
            //else if (register.Equals("$t2"))
            //    ret = "01010";
            //else if (register.Equals("$t3"))
            //    ret = "01011";
            //else if (register.Equals("$t4"))
            //    ret = "01100";
            //else if (register.Equals("$t5"))
            //    ret = "01101";
            //else if (register.Equals("$t6"))
            //    ret = "01110";
            //else if (register.Equals("$t7"))
            //    ret = "01111";
            //else if (register.Equals("$s0"))
            //    ret = "10000";
            //else if (register.Equals("$s1"))
            //    ret = "10001";
            //else if (register.Equals("$s2"))
            //    ret = "10010";
            //else if (register.Equals("$s3"))
            //    ret = "10011";
            //else if (register.Equals("$s4"))
            //    ret = "10100";
            //else if (register.Equals("$s5"))
            //    ret = "10101";
            //else if (register.Equals("$s6"))
            //    ret = "10110";
            //else if (register.Equals("$s7"))
            //    ret = "10111";
            //else if (register.Equals("$t8"))
            //    ret = "11000";
            //else if (register.Equals("$t9"))
            //    ret = "11001";
            //else if (register.Equals("$k0"))
            //    ret = "11010";
            //else if (register.Equals("$k1"))
            //    ret = "11011";
            //else if (register.Equals("$gp"))
            //    ret = "11100";
            //else if (register.Equals("$sp"))
            //    ret = "11101";
            //else if (register.Equals("$fp") || register.Equals("$s8"))
            //    ret = "11110";
            //else if (register.Equals("$ra"))
            //    ret = "11111";
            //return ret;
        }
        public string getAddressForJBin (string address)
            //banco de dados de endereços virtual, converte para binário
        {
            string ret = "0";
            if (address.Equals("a"))
                ret = "0000000000000000000100";
            else if (address.Equals("A"))
                ret = "0000000000000000001000";
            else if (address.Equals("b"))
                ret = "0000000000000000001100";
            else if (address.Equals("B"))
                ret = "0000000000000000010000";
            else if (address.Equals("c"))
                ret = "0000000000000000010100";
            else if (address.Equals("C"))
                ret = "0000000000000000011000";
            else if (address.Equals("d"))
                ret = "0000000000000000011100";
            else if (address.Equals("D"))
                ret = "0000000000000000100000";
            else if (address.Equals("e"))
                ret = "0000000000000000100100";
            else if (address.Equals("E"))
                ret = "0000000000000000101000";
            else if (address.Equals("f"))
                ret = "0000000000000000101100";
            else if (address.Equals("F"))
                ret = "0000000000000000110000";
            else if (address.Equals("g"))
                ret = "0000000000000000110100";
            else if (address.Equals("G"))
                ret = "0000000000000000111000";
            else if (address.Equals("h"))
                ret = "0000000000000000111100";
            else if (address.Equals("H"))
                ret = "0000000000000001000000";
            return ret;
        }
        public string convertToHex (string binCode)
            //converte a string de código binário para código hex
        {
            string code = "0x";
            int cValue = 0;
            cValue = Convert.ToInt32(binCode, 2);
            code = code + Convert.ToString(cValue, 16);
            return code;
        }
    }
}
