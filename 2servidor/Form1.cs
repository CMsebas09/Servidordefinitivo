using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace _2servidor
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        private Thread serverThread;
        private List<TcpClient> clients = new List<TcpClient>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Solicitar al usuario que ingrese el puerto
            string portStr = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el puerto (máximo 6 dígitos):", "Puerto", "13008");

            // Validar que el puerto sea un número y tenga máximo 6 dígitos
            int port;
            if (!int.TryParse(portStr, out port) || port <= 0 || port > 999999)
            {

                MessageBox.Show("Pu erto inválido. Debe ser un número entre 1 y 999999.");

                MessageBox.Show("Puerto inválido. Debe ser un número entre 1 y 999999");

                return;
            }

            // Iniciar el servidor con el puerto ingresado
            serverThread = new Thread(() => StartServer(port));
            serverThread.Start();
            btnStart.Enabled = false;

        }
        private void StartServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Invoke((Action)(() => listViewClientes.Items.Add("Servidor iniciado en puerto " + port)));

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Invoke((Action)(() => listViewClientes.Items.Add(client.Client.RemoteEndPoint.ToString())));
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    BroadcastMessage(message, client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                clients.Remove(client);
                Invoke((Action)(() =>
                {
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        if (item.Text == client.Client.RemoteEndPoint.ToString())
                        {
                            listViewClientes.Items.Remove(item);
                            break;
                        }
                    }
                }));
                client.Close();
            }
        }
        private void BroadcastMessage(string message, TcpClient senderClient)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            foreach (var client in clients)
            {
                if (client != senderClient)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void listViewClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
