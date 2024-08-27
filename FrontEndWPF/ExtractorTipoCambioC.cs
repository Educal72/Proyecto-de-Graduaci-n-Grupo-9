
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AngleSharp.Html.Parser;

namespace Molino_POS
{
	public class ExtractorTipoCambioC
	{
		public string Fiva;
		public string Fservicio;
		public decimal Fcambio;
		public async Task<string> ObtenerTipoDeCambioConAngleSharpAsync()
		{
			try
			{
				// Inicializar HttpClient para obtener el contenido de la página
				using (var httpClient = new HttpClient())
				{
					string urlBanco = "https://gee.bccr.fi.cr/IndicadoresEconomicos/Cuadros/frmConsultaTCVentanilla.aspx"; // URL del banco
					var htmlContent = await httpClient.GetStringAsync(urlBanco);

					// Crear un parser HTML con AngleSharp
					var parser = new HtmlParser();
					var document = await parser.ParseDocumentAsync(htmlContent);

					// Buscar el elemento que contiene el tipo de cambio usando un selector CSS o XPath
					// Ejemplo usando un selector CSS, ajusta según la estructura real
					var rateElement = document.QuerySelector("#DG > tbody:nth-child(1) > tr:nth-child(5) > td:nth-child(4)"); // Reemplaza con el selector correcto

					if (rateElement != null)
					{
						return rateElement.TextContent.Trim();
					}
					else
					{
						return "Tipo de cambio no encontrado.";
					}
				}
			}
			catch (Exception ex) {
				FileRead();
				return Fcambio.ToString();
			}
		}
		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/imp_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/imp_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "IVA":
								Fiva = parts[1];
								break;
							case "Servicio":
								Fservicio = parts[1];
								break;
							case "Tipo Cambio":
								Fcambio = Convert.ToDecimal(parts[1]);
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}
	}
}
