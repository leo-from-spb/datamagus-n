using System;
using System.Collections.Generic;
using Gena.Code;
using Gena.Fun;

namespace Gena.CSharp;

/// <summary>
/// C# file.
/// </summary>
public class CsFile
{
    public readonly CsConstruction Con;

    /// <summary>
    /// Namespace.
    /// </summary>
    public string Namespace;

    /// <summary>
    /// File name (without path and extension).
    /// </summary>
    public string Name;


    /// <summary>
    /// The text that is placed at the top if the file.
    /// </summary>
    public string Header = CsConsts.DefaultHeader;


    public readonly SortedSet<string> Imports = new SortedSet<string>();

    public readonly List<CsClass> Classes = new();

    internal CsFile(CsConstruction con, string ns, string name)
    {
        Con       = con;
        Namespace = ns;
        Name      = name;
    }

    public CsClass NewClass(string name,
                            string? baseNames = null,
                            bool isStatic = false,
                            bool isAbstract = false,
                            bool isSealed = false)
    {
        var newClass = new CsClass(this, name, baseNames, isStatic, isAbstract, isSealed);
        Classes.Add(newClass);
        return newClass;
    }
}


/// <summary>
/// C# class.
/// </summary>
public class CsClass
{
    public readonly CsFile File;

    public string Name;

    public string? BaseNames;

    public bool IsStatic;
    public bool IsAbstract;
    public bool IsSealed;

    public readonly List<String> TypoParams = new();

    public readonly List<CsMethod> Methods = new();


    internal CsClass(CsFile file, string name, string? baseNames, bool isStatic, bool isAbstract, bool isSealed)
    {
        File = file;
        Name = name;
        BaseNames = baseNames;
        IsStatic = isStatic;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
    }

    public CsMethod NewMethod(string methodName,
                              string? returnType = null,
                              bool isStatic = false,
                              bool isAbstract = false)
    {
        var method = new CsMethod(this, methodName, returnType, isStatic, isAbstract);
        Methods.Add(method);
        return method;
    }
}



public class CsMethod
{
    public readonly CsClass Class;

    public string Name;

    public bool IsStatic;
    public bool IsAbstract;

    public string? ReturnType;

    public readonly List<CsArgument> Arguments = new();

    public readonly CodeBuilder ContentBuilder = new();

    public CsMethod(CsClass clazz, string name, string? returnType, bool isStatic, bool isAbstract)
    {
        Class           = clazz;
        Name            = name;
        IsStatic        = isStatic;
        IsAbstract      = isAbstract;
        this.ReturnType = returnType;
    }

    public CsArgument NewArgument(string name, string type, string? defaultValue = null)
    {
        var parameter = new CsArgument(Class, name, type, defaultValue);
        Arguments.Add(parameter);
        return parameter;
    }

    public string Content => ContentBuilder.Result;
}



public class CsArgument
{
    public readonly CsClass Class;

    public string Name;
    public string Type;

    public string? DefaultValue;

    public CsArgument(CsClass clazz, string name, string type, string? defaultValue)
    {
        Class = clazz;
        Name  = name;
        Type  = type;
        DefaultValue = defaultValue;
    }

    public string Spec => Type + ' ' + Name + DefaultValue.Wrap(prefix: " = ");

}