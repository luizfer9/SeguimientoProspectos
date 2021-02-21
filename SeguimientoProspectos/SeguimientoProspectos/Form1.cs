using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeguimientoProspectos
{
    public partial class Form1 : Form
    {
        ErrorProvider errorProvider = new ErrorProvider();
        private List<Models.Documentos> streamDataList = new List<Models.Documentos>();
        private List<Models.ListaProspectos> prospectosList = new List<Models.ListaProspectos>();
        private Models.Prospecto dataObjec;
        private Models.Estatus stats;
        private Connections.ConnectionSQL conn = new Connections.ConnectionSQL();
        bool notleaveCaptura = false;
        public Form1()
        {
            conn.readFile(AppDomain.CurrentDomain.BaseDirectory+"centralConnection.txt");
            InitializeComponent();
        }

        #region "Buttons"
        private void button1_Click(object sender, EventArgs e)
        {
            bool okValidation = true;
            DataTable resultTable = new DataTable();
            int resultInt;
            string query = "";

            earseErrors();
            foreach (Control controlText in tabPage1.Controls)
            {
                if (controlText is TextBox)
                {
                    okValidation &= ValidatingEmptyFields(controlText);
                }
            }
            okValidation &= emptyDocuments();

            if (okValidation)
            {
                resultInt = conn.ExecuteScalarInt("Exec proc_obtenDataDocs 1") + 1;
                conn.LLenaDateTable(ref resultTable, "Exec proc_obtenDataDocs 2");
                dataObjec = new Models.Prospecto(nombre.Text, apellido1.Text, apellido2.Text, calle.Text, numeroCasa.Text, colonia.Text, Int64.Parse(cP.Text), Int64.Parse(telefono.Text), rfc.Text, streamDataList);
                listToTable(ref resultTable, dataObjec.DocsList, resultInt);
                conn.BulkCopy(resultTable, "Documentos");
                query ="Exec proc_capturaProspecto '"+
                    dataObjec.NombrePros + "', '"+ 
                    dataObjec.PrimerApe + "', '"+ 
                    dataObjec.SegundoApe + "', '"+ 
                    dataObjec.CallePros + "', '"+ 
                    dataObjec.NumeroCasa + "', '"+ 
                    dataObjec.Colonia + "', "+ 
                    dataObjec.CodePost + ", '"+ 
                    dataObjec.TelPros + "', '"+ 
                    dataObjec.rfc1 + "', "+ 
                    resultInt + ", "+1+"";
                conn.Execute(query);
                
                MessageBox.Show("La captura a sido exitosa.", 
                    "Exitoso", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                dataObjec.clearProspecto();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lv in listView1.SelectedItems)
            {
                streamDataList.RemoveAt(lv.Index);
                lv.Remove();
            }
            emptyDocuments();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Models.Documentos doc;
            int index = 0;
            if (streamDataList == null)
            {
                streamDataList = new List<Models.Documentos>();
            }

            index = streamDataList.Count;            

            if (nombreDocText.Text == "")
            {
                errorProvider.SetError(nombreDocText, "Debe insertar un nombre antes de agregar un documento.");
            }
            else
            {
                ListViewItem lvi;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    doc = new Models.Documentos(nombreDocText.Text, openFileDialog1.FileName, uploadData());
                    streamDataList.Add(doc);
                    lvi = new ListViewItem(streamDataList[index].NombreDoc.ToString());
                    lvi.SubItems.Add(streamDataList[index].ArchivoDoc.ToString());
                    listView1.Items.Add(lvi);
                }
            }
            nombreDocText.ResetText();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            int index = 0;
            errorProvider.Clear();
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                index = item.Index;
            }
            cargaInf(prospectosList[index].IdPros);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            validaEvaluacion();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            int indx = 0;
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                indx = item.Index;
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string dir = path + "/temp/";
            string filePath = dir + streamDataList[indx].NombreDoc;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.WriteAllBytes(filePath, streamDataList[indx].StreamData);

            Process.Start(filePath);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            int index = 0;
            int selItem = estatusEv.SelectedIndex;
            int flgExec1 = 0, flgExec2 = 0;
            selItem++;
            errorProvider.Clear();
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                index = item.Index;
            }

            if (estatusEv.SelectedIndex.Equals(2))
                flgExec1 = conn.ExecuteScalarInt("Exec proc_actEstatus 1," + prospectosList[index].IdPros + "," + selItem + ",'" + obsEv.Text + "'");

            flgExec2 = conn.ExecuteScalarInt("Exec proc_actEstatus 2," + prospectosList[index].IdPros + "," + selItem + ",'" + obsEv.Text + "'");

            flgExec1 = flgExec1 + flgExec2;

            if (flgExec1 == 2)
                MessageBox.Show("Se guardo correctamente la evaluación.",
                    "Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Se se detecto un erro al guardar la evaluación, favor de revisar la bitacora(dataErrors) de errores en la DB",
                "Alert",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        #endregion

        #region "Validatings & keyPress & SelectedIndexChanged"
        public bool ValidatingEmptyFields(Control text)
        {
            bool allOk = true;

            if (text.Text == "" && text.AccessibleName != "NombreDocumento")
            {
                allOk = false;
                errorProvider.SetError(text, "Debe ingresar un " + text.AccessibleDescription);
            }

            return allOk;
        }
        private void nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(nombre,"");
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(nombre, "No se permiten numeros");
            }
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(nombre, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(nombre, "No se permiten simbolos");
            }
        }
        private void nombre_Validating(object sender, CancelEventArgs e)
        {
            if(nombre.Text=="")
            {
                errorProvider.SetError(nombre, "Debe ingresar "+nombre.AccessibleDescription);
            }
            if (nombre.Text.Length < 3)
            {
                errorProvider.SetError(nombre, "Debe ingresar " + nombre.AccessibleDescription+ " mayor a 3 letras");
            }
        }
        private void apellido1_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(apellido1, "");
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido1, "No se permiten numeros");
            }
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido1, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido1, "No se permiten simbolos");
            }
        }
        private void apellido2_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(apellido2, "");
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido2, "No se permiten numeros");
            }
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido2, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(apellido2, "No se permiten simbolos");
            }
        }
        private void calle_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(calle, "");
            
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(calle, "No se permiten simbolos");
            }
        }
        private void numeroCasa_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(numeroCasa, "");
            
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(numeroCasa, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(numeroCasa, "No se permiten simbolos");
            }
            if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(numeroCasa, "No se permiten espacios en blanco");
            }

        }
        private void colonia_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(colonia, "");
          
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(colonia, "No se permiten simbolos");
            }
        }
        private void cP_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(cP, "");
            
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(cP, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(cP, "No se permiten simbolos");
            }
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(cP, "No se permiten letras");
            }
            if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(cP, "No se permiten espacios en blanco");
            }
        }
        private void telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(telefono, "");
            
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(telefono, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(telefono, "No se permiten simbolos");
            }
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(telefono, "No se permiten simbolos");
            }
            if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(telefono, "No se permiten espacios en blanco");
            }
        }
        private void rfc_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider.SetError(rfc, "");
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(rfc, "No se permiten signos de puntuación");
            }
            if (Char.IsSymbol(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(rfc, "No se permiten simbolos");
            }
            if(Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(rfc, "No se permiten espacios en blanco");
            }
        }
        private void telefono_Validating(object sender, CancelEventArgs e)
        {
            if (telefono.Text == "")
            {
                errorProvider.SetError(telefono, "Debe ingresar " + nombre.AccessibleDescription);
            }
            if(telefono.Text.Length < 10|| telefono.Text.Length < 10)
            {
                errorProvider.SetError(telefono,"Debe ingresar un telefono a 10 digitos");
            }
        }
        private void apellido1_Validating(object sender, CancelEventArgs e)
        {
            if (apellido1.Text == "")
            {
                errorProvider.SetError(apellido1, "Debe ingresar " + apellido1.AccessibleDescription);
            }
            if (apellido1.Text.Length < 3)
            {
                errorProvider.SetError(apellido1, "Debe ingresar un " + apellido1.AccessibleDescription + " mayor a 3 letras");
            }
        }
        private void calle_Validating(object sender, CancelEventArgs e)
        {
            if (calle.Text == "")
            {
                errorProvider.SetError(calle, "Debe ingresar " + calle.AccessibleDescription);
            }
            if (calle.Text.Length > 30)
            {
                errorProvider.SetError(calle, "Debe ingresar un " + calle.AccessibleDescription + " menor a 30 caracteres");
            }
        }
        private void numeroCasa_Validating(object sender, CancelEventArgs e)
        {
            if (numeroCasa.Text == "")
            {
                errorProvider.SetError(numeroCasa, "Debe ingresar " + numeroCasa.AccessibleDescription);
            }
            if (numeroCasa.Text.Length > 30)
            {
                errorProvider.SetError(numeroCasa, "Debe ingresar un " + numeroCasa.AccessibleDescription + " menor a 5 caracteres");
            }
        }
        private void colonia_Validating(object sender, CancelEventArgs e)
        {
            if (colonia.Text == "")
            {
                errorProvider.SetError(colonia, "Debe ingresar " + colonia.AccessibleDescription);
            }
        }
        private void cP_Validating(object sender, CancelEventArgs e)
        {
            if (cP.Text == "")
            {
                errorProvider.SetError(cP, "Debe ingresar " + cP.AccessibleDescription);
            }
        }
        private void rfc_Validating(object sender, CancelEventArgs e)
        {
            if (rfc.Text == "")
            {
                errorProvider.SetError(rfc, "Debe ingresar " + rfc.AccessibleDescription);
            }
        }
        private void obsEv_Validating(object sender, CancelEventArgs e)
        {
            if (estatusEv.SelectedIndex.Equals(2) && obsEv.Text == "")
                errorProvider.SetError(obsEv, "Debe escribir una razon para rechazo.");

        }
        private void estatusEv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (estatusEv.SelectedIndex.Equals(1))
                obsEv.Enabled = false;
            else
                obsEv.Enabled = true;
        }
        #endregion

        #region "Focus"
        private void tabPage1_Leave(object sender, EventArgs e)
        {
            DialogResult okCancel =
            MessageBox.Show("Se perderan los valores sin no han sido enviado, que desea hacer?",
                "Alerta",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (okCancel == DialogResult.OK)
            {
                foreach (Control controlText in tabPage1.Controls)
                {
                    if (controlText is TextBox)
                    {
                        controlText.ResetText();
                    }
                    if (controlText is ListView)
                    {
                        dataObjec.clearProspecto();
                        listView1.Items.Clear();
                        streamDataList.Clear();
                    }
                }
            }
            else
            {
                notleaveCaptura = true;
            }
        }
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            if (notleaveCaptura)
            {
                returnCaptura();
            }
            else
            {
                resetCaptura();
                tabListadoInit();
            }
        }
        private void resetCaptura()
        {
            nombre.ResetText();
            apellido1.ResetText();
            apellido2.ResetText();
            calle.ResetText();
            colonia.ResetText();
            numeroCasa.ResetText();
            cP.ResetText();
            telefono.ResetText();
            rfc.ResetText();
            listView1.Items.Clear();
        }
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            if (notleaveCaptura)
            {
                returnCaptura();
            }
            else
                tabEvaluacionInit();

            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Tiene que tener un prospecto seleccionado para evaluar",
                    "Alerta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                tabControl1.SelectedTab = tabControl1.TabPages[2];
            }
            else
            {
                validaEvaluacion();
            }
        }
        #endregion

        #region "Methods"
        public void earseErrors()
        {
            errorProvider.Clear();
        }
        public bool emptyDocuments()
        {
            errorProvider.SetError(listView1, "");
            if (listView1.Items.Count <= 0)
            {
                errorProvider.SetError(listView1, "Debe insertar almenos un documento para evaluar.");
                return false;
            }
            else
            {
                return true;
            }
        }
        private byte[] uploadData()
        {
            byte[] matByte = null;
            Stream myStream = openFileDialog1.OpenFile();
            using (MemoryStream ms= new MemoryStream())
            {
                myStream.CopyTo(ms);
                matByte = ms.ToArray();
            }

            return matByte;
        }
        private DataTable listToTable(ref DataTable table,List<Models.Documentos> pros,int indx)
        {
            DataRow dr;
            foreach (Models.Documentos docs in pros)
            {
                dr = table.NewRow();
                dr["id_docpros"] = indx;
                dr["desc_nombreDoc"] = docs.NombreDoc;
                dr["desc_rutaDoc"] = docs.ArchivoDoc;
                dr["bin_documento"] = docs.StreamData;
                table.Rows.Add(dr);
            }
            
            return table;
        }
        public void tabListadoInit()
        {
            listView2.Items.Clear();
            stats = null;
            dataObjec = null;
            List<Models.Estatus> statsList = new List<Models.Estatus>();
            DataTable result = new DataTable();
            conn.LLenaDateTable(ref result, "Exec proc_actEstatus 3,0,0,0");
            foreach (DataRow dr in result.Rows)
            {
                stats = new Models.Estatus(int.Parse(dr["id_estatus"].ToString()),dr["desc_estatus"].ToString());
                statsList.Add(stats);
            }
            stats.EstatusList = statsList;
            llenaListView2();
        }
        public void llenaListView2()
        {
            DataTable resultTable = new DataTable();
            Models.ListaProspectos itemPros;
            itemPros = null;
            ListViewItem viewItem;
            conn.LLenaDateTable(ref resultTable, "Exec proc_listaProspectos 1,0");
            foreach (DataRow pros in resultTable.Rows)
            {
                itemPros = new Models.ListaProspectos(
                int.Parse(pros["id_prospecto"].ToString()),
                pros["desc_nombre"].ToString(),
                pros["desc_apellido1"].ToString(),
                pros["desc_apellido2"].ToString(),
                pros["desc_estatus"].ToString()
                );
                prospectosList.Add(itemPros);

                viewItem = new ListViewItem(itemPros.NombrePros);
                viewItem.SubItems.Add(itemPros.PrimerApe);
                viewItem.SubItems.Add(itemPros.SegundoApe);
                viewItem.SubItems.Add(itemPros.Estatus);
                listView2.Items.Add(viewItem);
            }
        }
        public void tabEvaluacionInit()
        {
            estatusEv.Items.Clear();
            object[] items;
            items = null;
            items= new object[3];
            for(int i = 0; i < stats.EstatusList.Count; i++)
            {
                items[i] = stats.EstatusList[i].Desc_estatus;
            }
            nombreEv.Text = nombreInf.Text;
            direccionEv.Text = nombreInf.Text;
            rfcEv.Text = rfcInf.Text;
            telEv.Text = telInf.Text;
            CpEv.Text = cPInf.Text;
            estatusEv.Items.AddRange(items);
            if (estatusInf.Text != stats.EstatusList[0].Desc_estatus)
            {
                estatusEv.Enabled = false;
                obsEv.Text = obsInf.Text;
                obsEv.Enabled = false;
                button6.Enabled = false;
            }
            else
            {
                estatusEv.Enabled = true;
                obsEv.Enabled = true;
                button6.Enabled = true;
            }
        }
        public void validaEvaluacion()
        {
            if (listView2.Items.Count > 0)
            {
                if (listView2.SelectedItems.Count != 0)
                {
                    errorProvider.Clear();

                    if (estatusInf.Text == stats.EstatusList[0].Desc_estatus.ToString())
                    {
                        obsEv.Enabled = true;
                        tabControl1.SelectedTab = tabControl1.TabPages[2];
                    }
                }
                else
                {
                    errorProvider.SetError(button5, "Debe seleccionar un prospecto antes.");
                }

            }
        }
        private void cargaInf(int idPros)
        {
            DataTable resultTable = new DataTable();
            ListViewItem viewItem;
            Models.Documentos docs;
            string nombre, ap1, ap2, calle, colonia, noCasa, rfc, obs;
            int numDoc = 1,idDoc,idEstatus;
            Int64 tel, codPost;
            streamDataList = new List<Models.Documentos>();
            conn.LLenaDateTable(ref resultTable, "Exec proc_listaProspectos 2,"+idPros);

            nombre = resultTable.Rows[0]["desc_nombre"].ToString();
            ap1 = resultTable.Rows[0]["desc_apellido1"].ToString();
            ap2 = resultTable.Rows[0]["desc_apellido2"].ToString();
            nombreInf.Text = nombre + " " + ap1 + " " + ap2;
            calle = resultTable.Rows[0]["desc_calle"].ToString();
            noCasa = resultTable.Rows[0]["desc_numero"].ToString();
            colonia = resultTable.Rows[0]["desc_colonia"].ToString();
            direccionInf.Text = "Calle. " + calle + ". #" + noCasa + " Col." + colonia;
            tel = Int64.Parse(resultTable.Rows[0]["num_telefono"].ToString());
            telInf.Text = tel.ToString();
            codPost = Int64.Parse(resultTable.Rows[0]["num_codPost"].ToString());
            cPInf.Text = codPost.ToString();
            rfcInf.Text = rfc = resultTable.Rows[0]["desc_rfc"].ToString();
            obsInf.Text = obs = resultTable.Rows[0]["desc_observ"].ToString();
            idDoc = int.Parse(resultTable.Rows[0]["num_documentoProc"].ToString());
            idEstatus = int.Parse(resultTable.Rows[0]["num_estatus"].ToString());

            foreach (Models.Estatus item in stats.EstatusList)
            {
                if (item.Id_estatus.Equals(idEstatus))
                {
                    estatusInf.Text = item.Desc_estatus;
                    break;
                }
            }
            resultTable.Reset();
            conn.LLenaDateTable(ref resultTable, "Exec proc_listaProspectos 3," + idPros);

            foreach (DataRow dr in resultTable.Rows)
            {
                docs = new Models.Documentos(dr["desc_nombreDoc"].ToString(),
                    dr["desc_rutaDoc"].ToString(),
                   (byte[])dr["bin_documento"]);
                streamDataList.Add(docs);

                viewItem = new ListViewItem(numDoc.ToString());
                viewItem.SubItems.Add(dr["desc_nombreDoc"].ToString());
                listView3.Items.Add(viewItem);
                numDoc++;
            }
            dataObjec = new Models.Prospecto(nombre, ap1, ap2, calle, noCasa, colonia, codPost, tel, rfc, streamDataList);
            dataObjec.Obsrv = obs;
            dataObjec.NumDocs = idDoc;
            dataObjec.NumEstatus = idEstatus;
        }
        private void returnCaptura()
        {
            notleaveCaptura = false;
            tabControl1.SelectedTab = tabControl1.TabPages[0];
        }
        #endregion
    }
}
