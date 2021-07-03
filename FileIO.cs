using UnityEngine;
using System.IO;
using System.Collections.Generic;

//Class to define all FileIO functions used in the project, used to load, create and edit spell bytecode files
public static class FileIO
{
    static string folderPath = "C:\\Users\\George Fiddes\\Documents\\Game Design\\Unity Projects\\Design Pattern Prototypes\\ByteCode - Magical Scripting\\Bytecode - Magical Scripting\\Assets\\Spell Files";

    //Return the bytecode from the specified file
    public static int[] GetByteCode(string filePath)
    {
        List<string> lines = new List<string>();

        int i = 0;
        StreamReader reader = new StreamReader(filePath);
        try
        {
            do
            {
                lines.Add(reader.ReadLine());
                i++;
            }
            while (reader.Peek() != -1);
        }
        catch
        {
            Debug.Log(i);
        }
        reader.Close();
        

        int[] byteCode = new int[lines.Count];
        for(int a = 0; a < byteCode.Length; a++)
        {
            byteCode[a] = int.Parse(lines[a]);
        }
        return byteCode;
    }

    //Return list of spell bytecode arrays and spell names from the spells folder
    public static List<int[]> LoadSpellFiles(out List<string> spellNames)
    {
        //Get array of file paths
        string[] filePaths = Directory.GetFiles(folderPath);

        //Iterate through array of file paths to build list of byte codes and spell names
        List<int[]> bytecodes = new List<int[]>();
        List<string> names = new List<string>();
        foreach(string filePath in filePaths)
        {
            if(Path.GetExtension(filePath) == ".txt")
            {
                bytecodes.Add(GetByteCode(filePath));
                names.Add(Path.GetFileNameWithoutExtension(filePath));
            }
        }
        //Return list of bytecodes and spell names
        spellNames = names;
        return bytecodes;
    }
}
