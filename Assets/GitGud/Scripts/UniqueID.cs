using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueID
{
    public static List<string> codes = new List<string>() { "test" };
    public const string SRC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public const int CODE_LENGTH = 4;

    private System.Random RNG;

    private string code;

    public string Code { get => code; set => code = value; }


    public UniqueID() { RNG = new System.Random(); }

    public void GenerateCode() {

        do {
            code = "";
            for (int i = 0; i < CODE_LENGTH; i++) {
                char next = SRC[RNG.Next(0, SRC.Length)];
                code += next;
            }
        } while (codes.Contains(code));

        codes.Add(code);
    }
}
