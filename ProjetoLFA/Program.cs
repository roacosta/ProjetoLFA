﻿using System;
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
            public bool valida;
            public ItemRegra()
            {
                valida = true;
            }
        }

        public class Regra
        {
            public String nomeRegra;
            public List<ItemRegra> transicoes = new List<ItemRegra>();
            public Boolean final;
            public Boolean valida;

            public Regra()
            {
                this.final = false;
                this.valida = true;
            }            
        }

        public class ItemRegraAdicionar
        {
            public String nomeRegra;
            public Char simbolo;
            public String regraTransicao;
        }

        public class ContadorTransicao
        {
            public String regraAssociada;
            public int quantidade;
            public Char simbolo;
            public List<String> regras;

            public ContadorTransicao(){
                regras = new List<String>();
            }
        }

        

        public class GramaticaRegular
        {
            public String nomeEstado;
            public String linhaInteira;
        }


        static void Main(string[] args)
        {
            String[] nomesParaNovasRegras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "W", "Y", "Z" };
            List<GramaticaRegular> gramaticasRegulares = new List<GramaticaRegular>();
            List<String> tokens = new List<String>();
            List<Char> alfabeto = new List<Char>();
            List<String> nomesEstados = new List<String>();
            List<Regra> regras = new List<Regra>();
            List<ItemRegraAdicionar> transicoesAdicionar = new List<ItemRegraAdicionar>();
            List<ContadorTransicao> contadorTransicoes = new List<ContadorTransicao>();

            String estadoInicial = "S";

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
                /// Definir Estado 

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
                                    case '=':
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
                foreach (String estado in nomesEstados)
                {
                    Regra regra = new Regra();
                    regra.nomeRegra = estado;
                    regras.Add(regra);
                }

                foreach (GramaticaRegular gramatica in gramaticasRegulares)
                {
                    int read = 0;
                    int comecarLeitura = 0;
                    Char simbolo = ' ';
                    String transicao = "";
                    foreach (char caractere in gramatica.linhaInteira)
                    {

                        /* READ = 1: Lendo terminal
                         * READ = 2: Lendo Não-terminal
                         * READ = 3: Adicionar a produção à regra
                         */

                        switch (caractere)
                        {
                            case '=':
                                read = 1;
                                comecarLeitura = 1;
                                break;
                            case '|':
                                if (transicao.Length > 0 || !simbolo.Equals('\0'))
                                {
                                    read = 3;
                                }
                                else
                                {
                                    read = 1;
                                }

                                break;
                            case '<':
                                if (comecarLeitura == 1)
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
                                        if (transicao == "") regra.final = true;

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
                                    read = 1;
                                    transicao = "";
                                    simbolo = '\0';
                                }
                                break;
                        }
                    }
                    // Tratar última produção quando ainda não tratada
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
                foreach (var token in tokens)
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
                            if (!nomesEstados.Contains(novoNomeExterno))
                                nomesEstados.Add(novoNomeExterno);
                            Regra novaRegra = new Regra();
                            novaRegra.transicoes = new List<ItemRegra>();
                            novaRegra.nomeRegra = novoNomeExterno;
                            regras.Add(novaRegra);
                        }
                    }
                }
            }
            //====== Identificar estados finais
            {
                foreach (var regra in regras)
                {
                    if (regra.transicoes.Count == 0) regra.final = true;
                    foreach(var transicao in regra.transicoes)
                    {
                        if (transicao.regraTransicao.Length == 0)
                        {
                            transicao.regraTransicao = "X";
                        }
                    }
                }
            }
            //====== Imprimir regras
            {
                ImprimirRegras(regras);
            }
            //====== Remover Epsilon Transições
            {

                foreach (var regra in regras)
                {
                    foreach (var transicao in regra.transicoes)
                    {
                        if (transicao.simbolo.Equals('\0'))
                        {
                            var nomeRegraDestino = transicao.regraTransicao;
                            foreach (var regraDestino in regras)
                            {
                                if (regraDestino.nomeRegra == nomeRegraDestino)
                                {
                                    foreach (var transicaoRegraDestino in regraDestino.transicoes)
                                    {
                                        ItemRegraAdicionar transicaoAdicionar = new ItemRegraAdicionar
                                        {
                                            nomeRegra = regra.nomeRegra,
                                            regraTransicao = transicaoRegraDestino.regraTransicao,
                                            simbolo = transicaoRegraDestino.simbolo
                                        };
                                        transicoesAdicionar.Add(transicaoAdicionar);
                                    }
                                }
                            }
                            transicao.valida = false;
                        }
                    }
                }
                foreach (var transicao in transicoesAdicionar)
                {
                    ItemRegra novaTransicao = new ItemRegra
                    {
                        regraTransicao = transicao.regraTransicao,
                        simbolo = transicao.simbolo
                    };

                    Regra regra = BuscarRegra(regras, transicao.nomeRegra);
                    if (!VerificaExistenciaTransicaoRegra(regra, novaTransicao))
                        regra.transicoes.Add(novaTransicao);
                }


            }

            ImprimirRegras(regras);

        
            //====== Determinização
            {
                Console.WriteLine("Determinização: ");

                
                bool alterou = true;


                while (alterou)
                {
                    contadorTransicoes.Clear();
                    alterou = false;
                    foreach (var regra in regras)
                    {
                        if (regra.valida)
                        {
                            foreach (var transicao in regra.transicoes)
                            {
                                if (transicao.valida)
                                {
                                    var achou = 0;
                                    foreach (var contador in contadorTransicoes)
                                    {
                                        if (transicao.simbolo == contador.simbolo && regra.nomeRegra == contador.regraAssociada)
                                        {
                                            achou = 1;
                                            contador.quantidade++;
                                            contador.regras.Add(transicao.regraTransicao);
                                        }
                                    }
                                    if (achou == 0)
                                    {
                                        ContadorTransicao novoContador = new ContadorTransicao
                                        {
                                            regraAssociada = regra.nomeRegra,
                                            simbolo = transicao.simbolo,
                                            quantidade = 1
                                        };
                                        novoContador.regras.Insert(0, transicao.regraTransicao);
                                        contadorTransicoes.Add(novoContador);
                                    }
                                }
                            }
                        }
                    }


                    foreach (var contador in contadorTransicoes)
                    {
                        Console.WriteLine("Regra: " + contador.regraAssociada + " simbolo: " + contador.simbolo);
                        foreach (String regra in contador.regras)
                        {
                            Console.Write(" <" + regra + "> ");
                        }
                        Console.WriteLine("");
                    }

                    foreach (var contador in contadorTransicoes)
                    {
                        if (contador.quantidade > 1)
                        {
                            
                            String nomeNovaRegra = "";
                            foreach (String s in contador.regras)
                            {
                                nomeNovaRegra += s;
                            }
                            if (!nomesEstados.Contains(nomeNovaRegra))
                            {
                                nomesEstados.Add(nomeNovaRegra);

                                alterou = true;
                                Regra novaRegra = new Regra();
                                novaRegra.nomeRegra = nomeNovaRegra;
                                foreach (String s in contador.regras)
                                {
                                    foreach (var regra in regras)
                                    {
                                        if (regra.nomeRegra == s)
                                        {
                                            foreach (var transicao in regra.transicoes)
                                            {
                                                novaRegra.transicoes.Add(transicao);
                                            }
                                        }
                                    }
                                }

                                regras.Add(novaRegra);
                                foreach (var regra in regras)
                                {
                                    if (contador.regraAssociada == regra.nomeRegra)
                                    {
                                        ItemRegra novaTransicao = new ItemRegra();
                                        novaTransicao.regraTransicao = nomeNovaRegra;
                                        novaTransicao.simbolo = contador.simbolo;
                                        regra.transicoes.Add(novaTransicao);

                                        foreach (var transicao in regra.transicoes)
                                        {
                                            if (transicao.simbolo == contador.simbolo && transicao.regraTransicao != nomeNovaRegra)
                                            {
                                                transicao.valida = false;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    foreach (var regraChamada in regras)
                    {
                        int chamada = 0;
                        foreach (var outraRegra in regras)
                        {
                            if (regraChamada.nomeRegra != outraRegra.nomeRegra)
                            {
                                foreach (var transicao in outraRegra.transicoes)
                                {
                                    if (transicao.regraTransicao == regraChamada.nomeRegra && transicao.valida)
                                    {
                                        chamada = 1;
                                    }
                                }
                            }
                        }
                        if (chamada == 0 && regraChamada.nomeRegra != estadoInicial)
                        {
                            regraChamada.valida = false;
                        }
                    }
                }

                

                ImprimirRegras(regras);
            }
            String text = ";";
            foreach(char letra in alfabeto)
            {
                text += letra.ToString() + ';';
            }

            text += "\r\n";

            foreach(String estado in nomesEstados)
            {
                
                foreach(var regra in regras)
                {
                    if (regra.valida && regra.nomeRegra == estado)
                    {
                        text += estado + ';';
                        foreach (char letra in alfabeto)
                        {
                            foreach (var transicao in regra.transicoes)
                            {
                                if (transicao.valida) {
                                    if (transicao.simbolo == letra)
                                    {
                                        text += transicao.regraTransicao;
                                    }
                                }
                            }
                            text += ";";

                        }
                        text += "\r\n";
                    }
                }
            }
            System.IO.File.WriteAllText(@"Saida.csv", text);

        }

        public static Regra BuscarRegra(dynamic regras, String nomeRegra)
        {
            foreach (var regra in regras)
            {
                if (nomeRegra == regra.nomeRegra)
                {
                    return regra;
                }
            }
            return null;
        }

        public static Boolean VerificaExistenciaTransicaoRegra(Regra regra, ItemRegra transicao)
        {
            foreach (var transicaoRegra in regra.transicoes)
            {
                if(transicao.regraTransicao == transicaoRegra.regraTransicao && transicao.simbolo.Equals(transicaoRegra.simbolo))
                {
                    return true;
                }
            }
            return false;
        }

        public static void ImprimirRegras(dynamic regras)
        {
            foreach (var regra in regras)
            {
                if (regra.valida)
                {
                    if (regra.final == true)
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine("Regra " + regra.nomeRegra);
                    foreach (var transicoes in regra.transicoes)
                    {
                        if (transicoes.valida)
                            Console.Write(transicoes.simbolo + " < " + transicoes.regraTransicao + " > ");
                    }
                    Console.WriteLine("");
                }
            }
        }
        
    }
}
