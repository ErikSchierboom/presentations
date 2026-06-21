using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10;
                    """;

var tokens = new Scanner(code).Scan();
var tree = new Parser(tokens).Parse();
var instructions = new Compiler(tree).Compile();
var result = new VirtualMachine(instructions).Run();
Console.WriteLine(result);
