open System
open System.Diagnostics
open System.IO
open System.Text

let cDate = DateTime.Now.ToString("yyyyMMdd_mmss")
let pList = Process.GetProcesses()
let sba = new StringBuilder()
pList |> Seq.iter(fun item -> sba.AppendLine(String.Format("{0}. {1}", item.Id, item.ProcessName)) |> ignore)
printfn("Getting process list ")
File.WriteAllText(@"C:\Users\povil\source\repos\fsharplab\CurrentProcessList.txt", sba.ToString())
printfn("Writting process list ")
let sb = StringBuilder()
sb.AppendLine("ProcessID,ProcessName") |> ignore

let processes = Process.GetProcesses()
processes |> Seq.sortBy(fun p -> p.TotalProcessorTime) |> Seq.iter(fun p -> sb.AppendLine(String.Format("{0},{1}", p.ProcessName, p.Id)) |> ignore)
printfn("Getting processes by total time list ")

System.IO.File.WriteAllText(String.Format("C:\Users\povil\source\repos\fsharplab\TopProcessList.csv", cDate), sb.ToString())
printfn("Writting process by total time list ")

let searchAndCloseNotepadInstances = let notepadInstances = Process.GetProcessesByName("notepad")
                                     if (notepadInstances.Length > 0) then
                                        notepadInstances |> Seq.iter(fun notepadInstace -> Console.WriteLine("Notepad isjungiamas" + notepadInstace.Id.ToString()))
                                     else
                                        Console.WriteLine("Notepadas neveikia")

searchAndCloseNotepadInstances
printfn("Searching and killing notepad")
Console.ReadLine()