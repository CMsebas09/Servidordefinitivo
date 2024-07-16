using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente_Grafico
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private string clientName;
        private Thread receiveThread;
        private CancellationTokenSource cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void SendMessage(string message)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                // Agregar mensaje enviado al listView1 (opcional)
                AddMessageToListView(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
        }

        private void AddMessageToListView(string message)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke((MethodInvoker)(() =>
                {
                    listView1.Items.Add(message);
                }));
            }
            else
            {
                listView1.Items.Add(message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Llamar a button3_Click para manejar el cierre de la conexión
            button3_Click(sender, e);
        }

        private void ReceiveMessages(CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while (!token.IsCancellationRequested)
                {
                    if (stream.DataAvailable)
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            AddMessageToListView(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recibir mensaje: " + ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;
            SendMessage(clientName + " : " + message);
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario que ingrese la dirección IP y el puerto
                string ipAddress = Prompt.ShowDialog("Ingrese la IP:", "Configuración de conexión");
                int port;
                while (true)
                {
                    string portInput = Prompt.ShowDialog("Ingrese el puerto:", "Configuración de conexión");
                    if (int.TryParse(portInput, out port))
                        break;
                    else
                        MessageBox.Show("El puerto ingresado no es válido. Por favor, ingrese un número entero válido.");
                }

                // Conectar al servidor utilizando la dirección IP y el puerto ingresados
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();

                // Solicitar al usuario que ingrese su nombre de usuario
                clientName = Prompt.ShowDialog("Ingrese usuario:", "Nombre de Usuario");
                SendMessage("se ha conectado al chat como " + clientName);

                // Crear un nuevo CancellationTokenSource para la recepción de mensajes
                cancellationTokenSource = new CancellationTokenSource();
                receiveThread = new Thread(() => ReceiveMessages(cancellationTokenSource.Token));
                receiveThread.Start();

                // Habilitar el envío de mensajes al servidor
                textBox1.Enabled = true;
                button1.Enabled = true;

                // Deshabilitar el botón para evitar múltiples clics
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar al servidor: " + ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Evita que se escriba el salto de línea en el TextBox
                SendMessage(clientName + ": " + textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Enviar un mensaje al servidor indicando que se está desconectando
                SendMessage("se ha desconectado del chat.");

                // Cerrar el flujo de datos y el cliente

                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }

                if (client != null)
                {
                    client.Close();
                    client = null;
                }

                // Detener el hilo de recepción de mensajes
                if (receiveThread != null && receiveThread.IsAlive)
                {
                    cancellationTokenSource?.Cancel();
                    receiveThread.Join();
                    receiveThread = null;
                }

                // Deshabilitar el envío de mensajes al servidor
                textBox1.Enabled = false;
                button1.Enabled = false;

                // Habilitar el botón de iniciar para permitir una nueva conexión
                button2.Enabled = true;

                // Opcional: Puedes agregar un mensaje para informar al usuario que se ha desconectado
                MessageBox.Show("Se ha desconectado del chat.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desconectar del servidor: " + ex.Message);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "Aceptar", Left = 100, Width = 100, Top = 70, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
