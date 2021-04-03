using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueID
{
    public static List<string> codes = new List<string>();
    public const string SRC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public const int CODE_LENGTH = 4;

    private System.Random RNG;

    [SerializeField]
    private string code;

    public string Code { get => code; set => code = value; }


    public UniqueID() { RNG = new System.Random(); }

    public void GenerateCode(string _code = "") {
        
        // If code already generated, return
        if (!string.IsNullOrEmpty(code)) {
            return;
        }

        // If parameter code is invalid or already registered, print error.
        // else set code and add to list of codes.
        if (!string.IsNullOrEmpty(_code)) {
            if (_code.Length != CODE_LENGTH) {
                Debug.LogError("Trying to create code with invalid length");
            } else if(codes.Contains(_code)) {
                Debug.LogError("Trying to add code that allready exists");
            } else {
                codes.Add(_code);
                code = _code;
            }
        } else {
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
}
