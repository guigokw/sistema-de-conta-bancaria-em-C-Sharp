using Microsoft.EntityFrameworkCore;
using Sistema_de_conta_bancaria;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sistema_de_conta_bancaria;

public class ContaBancaria
{

    private string _numeroConta;

    public string NumeroConta
    {
        get { return _numeroConta; }

        set
        {
            if (value.Length != 4)
            {
                throw new Exception("o numero da conta deve conter 4 digitos apenas");
            }
            _numeroConta = value;
        }
    }



    private string _nomeTitular;
    
    public string NomeTitular
    {
        get
        {
            return _nomeTitular;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("o nome do titular não pode ser vazio");
            }
            _nomeTitular = value;
        }
    }

    private double _saldo;

    public double Saldo
    {
        get
        {
            return _saldo;
        }
        set
        {
            if (value < 0)
            {
                throw new Exception("o saldo inicial não pode ser negativo");
            }
            _saldo = value;
        }
    }


    public ContaBancaria(string numeroConta, string nomeTitular, double saldoInicial)
    {

        if (numeroConta.Length != 4)
        {
            throw new Exception("o numero da conta deve conter 4 digitos apenas");
        }
        if (saldoInicial < 0)
        {
            throw new Exception("o saldo inicial não pode ser negativo");
        }
        if (string.IsNullOrEmpty(nomeTitular))
        {
            throw new Exception("o nome do titular não pode ser vazio");
        }
        this._numeroConta = numeroConta;
        this._nomeTitular = nomeTitular;
        this._saldo = saldoInicial;

    }

    public void exibirDetalhes()
    {
        Console.WriteLine("NUMERO DA CONTA: " + this._numeroConta);
        Console.WriteLine("NOME DO TITULAR: " + this._nomeTitular);
        Console.WriteLine("SALDO DO TITULAR: " + this._saldo);
    }

}

class BancoService
{
    List<ContaBancaria> contas = new List<ContaBancaria>();
    HashSet<string> numero = new HashSet<string>();
    public void adicionarConta()
    {

        try
        {
            Console.WriteLine("Digite o numero da conta (4 digitos): ");
            string numeroConta = Console.ReadLine();

            Console.WriteLine("Digite o nome do titular: ");
            string nomeTitular = Console.ReadLine();

            Console.WriteLine("Digite o saldo inicial: ");
            double saldoInicial = double.Parse(Console.ReadLine());

            if (numero.Contains(numeroConta))
            {
                throw new Exception("o numero da conta já existe, por favor escolha outro");
            }
            else
            {
                ContaBancaria conta = new ContaBancaria(numeroConta, nomeTitular, saldoInicial);
                contas.Add(conta);
                numero.Add(numeroConta);


               using (var contexto = new BancoContext())
                {
                    contexto.contass.Add(conta);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

        }
    }

    public void removerConta()
    {
        if (contas.Count == 0)
        {
            Console.WriteLine("não foi possível remover a conta pois a lista de contas bancárias está vazia");
        }
        else
        {
            Console.WriteLine("Digite o numero da conta que deseja remover: ");
            string numeroDaConta = Console.ReadLine();
            ContaBancaria contaRemover = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta);
            if (contaRemover != null)
            {
                contas.Remove(contaRemover);
                numero.Remove(contaRemover.NumeroConta);

                using (var context = new BancoContext())
                {
                    context.contass.Remove(contaRemover);
                    context.SaveChanges();
                }
                Console.WriteLine("Conta removida com sucesso.");
            }
            else
            {
                Console.WriteLine("Conta não encontrada.");
            }
        }
    }

    public void depositar()
    {
        try
        {
            if (contas.Count == 0)
            {
                Console.WriteLine("não foi possível realizar o depósito pois não há nenhuma conta pra depositar");
            }
            else
            {
                Console.WriteLine("digite o numero da conta que você deseja depositar");
                string numeroDaConta = Console.ReadLine();
                ContaBancaria contaDeposito = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta);

                if (contaDeposito != null)
                {
                    Console.WriteLine("==== DETALHES DA CONTA =====");
                    contaDeposito.exibirDetalhes();
                    Console.WriteLine("============================");

                    Console.WriteLine("1 - sim");
                    Console.WriteLine("2 - não");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("essa é a conta que você deseja depositar?");
                    int opcao = int.Parse(Console.ReadLine());

                    switch (opcao)
                    {

                        case 1:
                            Console.WriteLine("qual o valor que você deseja depositar");
                            double valor = double.Parse(Console.ReadLine());

                            if (valor <= 0)
                            {
                                throw new Exception("não foi possível realizar o deposito pois o valor é menor que 0");
                            }
                            else
                            {
                                contaDeposito.Saldo += valor;
                                Console.WriteLine("deposito concluido");
                                break;
                            }
                        case 2:
                            Console.WriteLine("se você deseja depositar em outra conta, por favor insira novamente");
                            break;
                        default:
                            Console.WriteLine("opção inválida, por favor insira novamente");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("não foi possivel realizar o deposito pois a conta não foi encontrada");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void sacar()
    {
        try
        {
            if (contas.Count == 0)
            {
                Console.WriteLine("não foi possível sacar pois não há contas registradas no bancio");
            }
            else
            {
                Console.WriteLine("qual o numero da conta que você deseja sacar?");
                string numeroDaConta = Console.ReadLine();

                ContaBancaria contaSaque = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta);

                if (contaSaque != null)
                {
                    Console.WriteLine("==== Detalhes da conta =====");
                    contaSaque.exibirDetalhes();
                    Console.WriteLine("============================");

                    Console.WriteLine("1 - sim");
                    Console.WriteLine("2 - não");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("essa é a conta que você deseja sacar?");
                    int opcao = int.Parse(Console.ReadLine());

                    switch (opcao)
                    {
                        case 1:
                            Console.WriteLine("qual valor que você deseja sacar");
                            double valor = double.Parse(Console.ReadLine());

                            if (valor <= 0)
                            {
                                throw new Exception("não foi possível realizar o saque pois o valor é inválido");

                            }
                            else if (valor > contaSaque.Saldo)
                            {
                                throw new Exception("não foi possível realizar o saque pois o valor inserido é maior do que o saldo atual");
                            }
                            else
                            {
                                contaSaque.Saldo -= valor;
                                Console.WriteLine("saque sucecido!!!");
                            }
                            break;
                        case 2:
                            Console.WriteLine("se você deseja sacar de outra conta, por favor tente novamente");
                            break;
                        default:
                            Console.WriteLine("opção inválida, por favor insira novamente");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("conta não encontrada");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }
    }

    public void transferir()
    {
        try
        {
            if (contas.Count == 0)
            {
                Console.WriteLine("não foi possível fazer a transferencia pois não há contas registradas");
            }
            else
            {
                Console.WriteLine("qual o numero da conta que você deseja transferir?");
                string numeroDaConta = Console.ReadLine();

                Console.WriteLine("qual o número da conta que você deseja receber a transferência?");
                string numeroDaConta2 = Console.ReadLine();

                ContaBancaria conta1 = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta);
                ContaBancaria conta2 = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta2);

                if (conta1 == null || conta2 == null)
                {
                    throw new Exception("não foi possível realizar a transferência pois alguma das contas não foi encontrada");
                }
                else
                {
                    Console.WriteLine("qual o valor que você deseja transferir da conta de " + conta1.NomeTitular + " para a de " + conta2.NomeTitular);
                    double valor = double.Parse(Console.ReadLine());

                    if (valor > conta1.Saldo)
                    {
                        throw new Exception("não foi possível realizar a transferencia pois o valor da transferencia é maior que o saldo do cliente que deseja tarsnferir");
                    }
                    else if (valor <= 0)
                    {
                        throw new Exception("não foi possível realizar a trasnferencia pois o valor é menor ou igual a zero");
                    }
                    else
                    {
                        conta1.Saldo -= valor;
                        conta2.Saldo += valor;
                        Console.WriteLine("transferência concluída!!!");
                    }

                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void exibirDetalhesConta()
    {
        if (contas.Count == 0)
        {
            Console.WriteLine("não foi possível exibir detalhes pois nenhuma conta foi cadatsrada");
        }
        else
        {
            Console.WriteLine("qual o número da sua conta?");
            string numeroDaConta = Console.ReadLine();

         

           ContaBancaria contaCliente = contas.FirstOrDefault(c => c.NumeroConta == numeroDaConta);
            if (contaCliente != null)
            {
                Console.WriteLine("==== CONTA BANCÁRIA DE " + contaCliente.NomeTitular + "=====");
                contaCliente.exibirDetalhes();

            }
            else
            {
               Console.WriteLine("conta não encontrada");
            }
            
        }
    }


}


class Program
{
    static void Main()
    {

        BancoService banco = new BancoService();

        while (true)
        {
            try
            {
                Console.WriteLine("1 - adicionar conta");
                Console.WriteLine("2 - remover conta");
                Console.WriteLine("3 - depositar");
                Console.WriteLine("4 - realizar saque");
                Console.WriteLine("5 - realizar transferencia");
                Console.WriteLine("6 - exibir detalhes da conta");
                Console.WriteLine("7 - sair");
                Console.WriteLine("----------------------------");

                Console.WriteLine("qual dessa opções você deseja seguir?");
                int opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        banco.adicionarConta();
                        break;
                    case 2:
                        banco.removerConta();
                        break;
                    case 3:
                        banco.depositar();
                        break;
                    case 4:
                        banco.sacar();
                        break;
                    case 5:
                        banco.transferir();
                        break;
                    case 6:
                        banco.exibirDetalhesConta();
                        break;
                    case 7:
                        Console.WriteLine("saindo do programa....");
                        return;
                    default:
                        Console.WriteLine("opção inválida, por favor tente novamente");
                        break;

                }

            } catch (FormatException ex)
            {
                Console.WriteLine("entrada inválida, por favor digite novamente");
            }


        }   
    }
}






