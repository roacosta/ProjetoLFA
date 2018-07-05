using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoLFA
{
    class Program
    {
        public class ItemRegra
        {
            public Char simbolo;
            public String regraTransicao;
        }

        public class Regra
        {
            public String nomeRegra;
            public List<ItemRegra> transicoes = new List<ItemRegra>();
            
        }

        public class GramaticaRegular
        {
            public String nomeEstado;
            public String linhaInteira;
        }


        static void Main(string[] args)
        {
            String[] nomesParaNovasRegras = {"A","B", "C", "D" , "E", "F" , "G", "H" , "I", "J" , "K", "L" , "M", "N" , "O", "P" , "Q", "R" , "S", "T" , "U", "V" , "X", "W","Y","Z"};
            List<GramaticaRegular> gramaticasRegulares = new List<GramaticaRegular>();
            List<String> tokens = new List<String>();
            List<Char> alfabeto = new List<Char>();
            List<String> nomesEstados = new List<String>();
            List<Regra> regras = new List<Regra>();

            

            //===== Leitura do arquivo

            string[] linhasArquivo = System.IO.File.ReadAllLines(@"Entrada.txt");

            //====== Define os tokens
            {
                foreach (String linha in linhasArquivo)
                {
                    if ((linha.Length > 0) && (!linha.Contains("<")))
                    {
                        tokens.Add(linha);
                    }

                }
            }
            //====== Define os Estados 
            {
                foreach (String linha in linhasArquivo)
                {
                    if (linha.Length > 0)
                    {
                        if ((linha.Contains("<")) && (linha.Contains(">")))
                        {

                            GramaticaRegular gramaticaRegular = new GramaticaRegular();
                            gramaticaRegular.linhaInteira = linha;

                            String estado = "";
                            int read = 0;
                            int primeiro = 1;
                            foreach (char caractere in linha)
                            {
                                switch (caractere)
                                {
                                    case '<':
                                        read = 1;
                                        estado = "";
                                        break;
                                    case '>':
                                        if (!nomesEstados.Contains(estado))
                                            nomesEstados.Add(estado);
                                        if (primeiro == 1)
                                            gramaticaRegular.nomeEstado = estado;
                                        primeiro = 0;
                                        read = 0;
                                        break;
                                    default:
                                        if (read == 1)
                                            estado += caractere;
                                        break;
                                }
                            }
                            gramaticasRegulares.Add(gramaticaRegular);
                        }
                    }
                }

            }
            //====== Define o alfabeto
            {
            

                foreach (String linha in linhasArquivo)
                {
                    if (linha.Length > 0)
                    {
                        if ((linha.Contains("<")) && (linha.Contains(">")))
                        {

                            int read = 0;
                            foreach (char caractere in linha)
                            {
                                switch (caractere)
                                {
                                    case '|':
                                        read = 1;
                                        break;
                                    case '<':
                                        read = 0;
                                        break;
                                    case ' ':
                                        break;
                                    default:
                                        if ((read == 1) && (!alfabeto.Contains(caractere)))
                                            alfabeto.Add(caractere);
                                        break;
                                }
                            }
                        }
                    }
                }

                foreach (string token in tokens)
                {
                    foreach (char caractere in token)
                    {
                        if (!alfabeto.Contains(caractere))
                        {
                            alfabeto.Add(caractere);
                        };
                    }
                }
            }
            //====== Debug Estados e Alfabeto
            {
                Console.WriteLine("Estados");
                foreach (string estado in nomesEstados)
                {
                    Console.WriteLine('*' + estado + '*');
                }

                Console.WriteLine("Alfabeto");
                foreach (char letra in alfabeto)
                {
                    Console.WriteLine(letra.ToString() + ' ');
                }
            }

            //====== Define as Regras pelas gramáticas informadas no arquivo
            {
                foreach(String estado in nomesEstados)
                {
                    Regra regra = new Regra();
                    regra.nomeRegra = estado;
                    regras.Add(regra);
                }

                foreach (GramaticaRegular gramatica in gramaticasRegulares)
                {
                    int read = 0;
                    Char simbolo = ' ';
                    String transicao = "";
                    foreach (char caractere in gramatica.linhaInteira)
                    {

                        switch (caractere)
                        {
                            case '=':
                                read = 1;
                                break;
                            case '|':
                                read = 1;
                                break;
                            case '<':
                                if (read == 1)
                                    read = 2;
                                break;
                            case '>':
                                if (read == 2)
                                    read = 3;
                                break;
                            default:
                                if ((read == 1) && !caractere.Equals(' '))
                                {
                                    simbolo = caractere;
                                }
                                if ((read == 2) && !caractere.Equals(' '))
                                {
                                    transicao += caractere;
                                }
                                if (read == 3)
                                {
                                    ItemRegra itemRegra = new ItemRegra();
                                    itemRegra.simbolo = simbolo;
                                    itemRegra.regraTransicao = transicao;

                                    foreach (Regra regra in regras)
                                    {
                                        if (regra.nomeRegra.Equals(gramatica.nomeEstado))
                                        {
                                            try
                                            {
                                                regra.transicoes.Add(itemRegra);
                                            }
                                            catch
                                            {
                                                regra.transicoes.Insert(0, itemRegra);
                                            }

                                        }
                                    }
                                    read = 0;
                                    transicao = "";
                                }
                                break;                            
                        }
                    }
                    if (read == 3)
                    {
                        ItemRegra itemRegra = new ItemRegra();
                        itemRegra.simbolo = simbolo;
                        itemRegra.regraTransicao = transicao;

                        foreach (Regra regra in regras)
                        {
                            if (regra.nomeRegra.Equals(gramatica.nomeEstado))
                            {
                                try
                                {
                                    regra.transicoes.Add(itemRegra);
                                }
                                catch
                                {
                                    regra.transicoes.Insert(0, itemRegra);
                                }

                            }
                        }
                        read = 0;
                        transicao = "";
                    }                    
                }
            }

            //====== Define as Regras pelos tokens
            {
                var estadoAtual = "S";
                var temRegra = 0;
                foreach(var token in tokens)
                {
                    estadoAtual = "S";
                    foreach (var caractere in token)
                    {
                        var adicionarRegra = 0;
                        String novoNomeExterno = "";
                        foreach (var regra in regras)
                        {
                            if (regra.nomeRegra.Equals(estadoAtual))
                            {
                                

                                foreach (var novoNome in nomesParaNovasRegras)
                                {
                                    temRegra = 0;
                                    foreach (var regraTeste in regras)
                                    {
                                        if (regraTeste.nomeRegra == novoNome)
                                        {
                                            temRegra = 1;
                                            continue;
                                        }
                                    }
                                    if (temRegra == 0)
                                    {
                                        
                                        
                                        adicionarRegra = 1;
                                        novoNomeExterno = novoNome;
                                        estadoAtual = novoNome;

                                        ItemRegra itemRegra = new ItemRegra();
                                        itemRegra.regraTransicao = novoNome;
                                        itemRegra.simbolo = caractere;
                                        regra.transicoes.Add(itemRegra);
                                            
                                        break;
                                    }
                                }
                            }
                                
                        }
                        if (adicionarRegra == 1)
                        {
                            Regra novaRegra = new Regra();
                            novaRegra.transicoes = new List<ItemRegra>();
                            novaRegra.nomeRegra = novoNomeExterno;
                            regras.Add(novaRegra);                            
                        }
                    }
                }
            }
            foreach (var regra in regras)
            {
                Console.WriteLine("Regra " + regra.nomeRegra);
                foreach (var transicoes in regra.transicoes)
                {
                    Console.Write(transicoes.simbolo + " < " + transicoes.regraTransicao + " > ");
                }
                Console.WriteLine("");
            }
        }
        
    }
}
