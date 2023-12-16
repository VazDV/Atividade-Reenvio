using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade_Reenvio
{
    class Program
    {
        static void Main()
        {
            Process[] processes = InitializeProcesses();
            string processTablePath = "D:\\Atividade Reenvio\\table.txt";

            using (StreamWriter writer = new StreamWriter(processTablePath))
            {
                foreach (var process in processes)
                {
                    writer.WriteLine($"PID: {process.PID}, TP: {process.TP}, CP: {process.CP}, EP: {process.EP}, NES: {process.NES}, N_CPU: {process.N_CPU}");
                }
            }

            for (int i = 0; i < processes.Length; i++)
            {
                ExecuteProcess(processes[i], processTablePath);
            }
        }

        static Process[] InitializeProcesses()
        {
            Process[] processes = new Process[10];

            for (int i = 0; i < 10; i++)
            {
                processes[i] = new Process
                {
                    PID = i,
                    TP = GetProcessExecutionTime(i),
                    CP = 0,
                    EP = "PRONTO",
                    NES = 0,
                    N_CPU = 0
                };
            }

            return processes;
        }

        static int GetProcessExecutionTime(int PID)
        {
            int[] executionTimes = { 10000, 5000, 7000, 3000, 3000, 8000, 2000, 5000, 4000, 10000 };
            return executionTimes[PID];
        }

        static void ExecuteProcess(Process process, string processTablePath)
        {
            int quantum = 1000;

            while (process.TP < process.TP + quantum)
            {
                process.EP = "EXECUTANDO";
                process.N_CPU++;
                process.CP++;

                // Simular operação de E/S com 5% de probabilidade
                if (new Random().Next(100) < 5)
                {
                    process.EP = "BLOQUEADO";
                    process.NES++;
                    PrintProcessInfo(process, processTablePath, "EXECUTANDO >>> BLOQUEADO");

                    // Simular 30% de chance de voltar para PRONTO após E/S
                    if (new Random().Next(100) < 30)
                    {
                        process.EP = "PRONTO";
                        PrintProcessInfo(process, processTablePath, "BLOQUEADO >>> PRONTO");
                        break;
                    }
                }

                process.TP++;

                if (process.TP % quantum == 0)
                {
                    process.EP = "PRONTO";
                    PrintProcessInfo(process, processTablePath, "EXECUTANDO >>> PRONTO");
                    break;
                }
            }

            // Processo terminou sua execução
            if (process.TP == GetProcessExecutionTime(process.PID))
            {
                process.EP = "TERMINADO";
                PrintProcessInfo(process, processTablePath, "EXECUTANDO >>> TERMINADO");
            }
        }

        static void PrintProcessInfo(Process process, string processTablePath, string transition)
        {
            Console.WriteLine($"{transition}: PID={process.PID}, TP={process.TP}, CP={process.CP}, NES={process.NES}, N_CPU={process.N_CPU}");

            using (StreamWriter writer = new StreamWriter(processTablePath, true))
            {
                writer.WriteLine($"PID: {process.PID}, TP: {process.TP}, CP: {process.CP}, EP: {process.EP}, NES: {process.NES}, N_CPU: {process.N_CPU}");
            }
        }
    }
}
