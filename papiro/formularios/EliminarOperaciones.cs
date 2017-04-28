using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class EliminarOperaciones : Form
    {
        private readonly usuarios _user;

        private readonly List<int> _operacionesId;

        private readonly List<int> _clientesId;

        private readonly List<int> _tipOpId;

        private readonly List<int> _tipProdId;

        public EliminarOperaciones(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _operacionesId = new List<int>();
           _clientesId = new List<int>{-1};
            _tipOpId = new List<int> {-1};
            _tipProdId = new List<int> {-1};
        }

        private void ListarOperaciones()
        {
            try
            {
                listarOplistView.Items.Clear();
                _operacionesId.Clear();
                
                using (var entities = new papiro_finalEntities())
                {
                    int clientId = _clientesId[clienteToolStripComboBox.SelectedIndex];
                    int tipOpId = _tipOpId[tipoOptoolStripComboBox.SelectedIndex];
                    int tipProdId = _tipProdId[tipProdtoolStripComboBox.SelectedIndex];
                    foreach (var op in entities.operaciones.
                           Where(op => 
                                 (clienteToolStripComboBox.SelectedIndex == 0 || clientId == op.id_cliente) &&
                                 (tipoOptoolStripComboBox.SelectedIndex == 0 || tipOpId == op.id_tipo_operacion) &&
                                 (tipProdtoolStripComboBox.SelectedIndex == 0 || tipProdId == op.id_tipo_producto)))
                    {
                        if (op.fecha.Date != fecha_op.Value.Date) continue;
                        _operacionesId.Add(op.id);

                        listarOplistView.Items.Add(
                            new ListViewItem(new[]
                                                 {
                                                     op.cliente.nombre,
                                                     (op.contrato == null ? "" : op.contrato.descripcion),
                                                     (op.tipo_operacion == null ? "" : op.tipo_operacion.valor),
                                                     (op.tipo_producto == null ? "" : op.tipo_producto.valor),
                                                     op.fecha.ToString("dd/MM/yyyy"), op.descripcion,
                                                     Math.Round(op.monto, 2).ToString(CultureInfo.InvariantCulture)
                                                 }));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + @"Excepción: " + exception.Message,
                    "Listar operaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OperacionesLoad(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    clienteToolStripComboBox.Items.Clear();
                    clienteToolStripComboBox.Items.Add("(Cliente)");
                    foreach(var client in entities.cliente)
                    {
                        _clientesId.Add(client.id);
                        clienteToolStripComboBox.Items.Add(client.nombre);
                    }
                    clienteToolStripComboBox.SelectedIndex = 0;

                    tipoOptoolStripComboBox.Items.Clear();
                    tipoOptoolStripComboBox.Items.Add("(Tipo de operación)");
                    foreach (var tipoOp in entities.tipo_operacion)
                    {
                        _tipOpId.Add(tipoOp.id);
                        tipoOptoolStripComboBox.Items.Add(tipoOp.valor);
                    }
                    tipoOptoolStripComboBox.SelectedIndex = 0;

                    tipProdtoolStripComboBox.Items.Clear();
                    tipProdtoolStripComboBox.Items.Add("(Tipo de producto)");
                    foreach (var tipoProducto in entities.tipo_producto)
                    {
                        _tipProdId.Add(tipoProducto.id);
                        tipProdtoolStripComboBox.Items.Add(tipoProducto.valor);
                    }
                    tipProdtoolStripComboBox.SelectedIndex = 0;
                    
                    ListarOperaciones();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Eliminar Operaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            ListarOperaciones();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ListarOperaciones();
        }

        private void SalirbuttonClick(object sender, EventArgs e)
        {
            Close();
        }

        private static string OperacionToString(operaciones operacion)
        {
            string str = "Operación con Id: " + operacion.id + ", Cliente: " + operacion.cliente.nombre;

            if (operacion.contrato != null)
                str += ", Contrato: " + operacion.contrato.descripcion;

            if (operacion.tipo_operacion != null)
                str += ", Tipo de operación: " + operacion.tipo_operacion.valor + ", Valor operación: " +
                       Math.Round(operacion.valor_tipo_operacion.Value, 2);

            str += ", Monto: " + Math.Round(operacion.monto, 2);

            if (operacion.id_comercial != null)
                str += ", Comercial: " + operacion.usuarios.nombre;

            if (operacion.id_operador != null)
                str += ", Operador: " + operacion.usuarios2.nombre;

            if (operacion.tipo_producto != null)
                str += ", Tipo de producto: " + operacion.tipo_producto.valor;

            str += ", Cobrada: " + (operacion.pendiente == 0 ? "Si" : "No");

            return str;
        }

        private void EjecutaroperacionbuttonClick(object sender, EventArgs e)
        {
            // Eliminar las operaciones seleccionadas.
            
            var checkedListViewItemCollection = listarOplistView.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos una operación.", @"Eliminar operación",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar la operación seleccionada?",
                                    @"Eliminar operación", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar las operaciones seleccionadas?",
                                    @"Eliminar operaciones", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                //variables para llevar el control de los numeros del balance
                decimal gasto = 0;
                decimal costo = 0;
                decimal monto = 0;
                Boolean cXc = false;
                decimal gasto_3ero = 0;

                using (var entities = new papiro_finalEntities())
                {
                    foreach (operaciones opByKey in from ListViewItem checkedItem in checkedListViewItemCollection
                                                    select entities.GetObjectByKey(
                                                        new EntityKey("papiro_finalEntities.operaciones",
                                                                      "id", _operacionesId[checkedItem.Index])))
                    {
                        //Primero pregunto si esta en la misma operacion, de lo contrario es imposible eliminar la operacion
                        if(DateTime.Now.Day - opByKey.fecha.Day > 7)
                        {
                            MessageBox.Show(
                                "No se puede eliminar la operación, pues ya se realizó un cierre de cuentas nominales",
                                "No se puede eliminar la operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        
                        //Pregunto si la operacion3ero esta pendiente si ya se efectuo la cuenta por pagar no se puede eliminar
                        //la oepracion
                        if (opByKey.cuentas_por_pagar.Any(cp => cp.ejecutada == 1 && cp.id_operacion == opByKey.id))
                        {
                            MessageBox.Show(
                                           "No se ha podido llevar a cabo la operación porque la operación("+ opByKey.descripcion +") con tercers.\n"
                                           + "ya fue pagada.",
                                           "Eliminar Operaciones",
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        if (opByKey.pendiente_imprimir == 0)    // Si está pendiente se pasa a eliminar.
                        {
                            // Obtener último balance para actualizar.
                            balance balance = entities.balance.ToList().Last();
                            
                            // Cuando es un diseño el tipo de operación no está definido.
                            if (opByKey.id_tipo_operacion == null || opByKey.id_tipo_operacion == 0)
                            {
                                // Modifico el ingreso correspondiente para la misma fecha de la operación.
                                submayor_ingreso subM_ingreso = entities.submayor_ingreso.Where(
                                    sub_ingreso => sub_ingreso.fecha.Day == opByKey.fecha.Day
                                        && sub_ingreso.fecha.Month == opByKey.fecha.Month
                                        && sub_ingreso.fecha.Year == opByKey.fecha.Year
                                        && sub_ingreso.credito == opByKey.monto && sub_ingreso.debito == null).Last();
                                subM_ingreso.credito = 0;
                                subM_ingreso.debito = 0;
                                subM_ingreso.saldo = 0;
                                subM_ingreso.descripcion = "Se elimina operación del dia " + subM_ingreso.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                subM_ingreso.fecha = DateTime.Now;

                                submayor_efectivo_caja efc  = entities.submayor_efectivo_caja.Where(
                                    efectivo => efectivo.fecha.Day == opByKey.fecha.Day
                                                && efectivo.fecha.Month == opByKey.fecha.Month
                                                && efectivo.fecha.Year == opByKey.fecha.Year
                                                && efectivo.debito == opByKey.monto && efectivo.credito == null).Last();
                                efc.credito = 0;
                                efc.debito = 0;
                                efc.saldo = 0;
                                efc.descripcion = "Se elimina operación del dia " + efc.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                efc.fecha = DateTime.Now;

                                // Capturo el ingreso
                                monto = opByKey.monto;

                                 // Quitar en el salario extra del user el % por operación del monto.
                                var user =
                                    (usuarios)
                                    entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                          _user.id));
                                user.salario_extra_operaciones -= opByKey.monto*(decimal) user.por_ciento_operaciones/
                                                                  100;
                                gasto += opByKey.monto * (decimal)user.por_ciento_operaciones /
                                                                  100;

                                //Le quito los valores a los balances desde el dia en que se elimina la operacion hasta el dia actual.
                                //Elimino de cada balance las utilidades que represento la operacion
                                foreach (var balan in entities.balance.Where(balance1 => balance1.fecha.Value.Year == opByKey.fecha.Year
                                    && balance1.fecha.Value.Month == opByKey.fecha.Month && balance1.fecha.Value.Day == opByKey.fecha.Day
                                    || balance1.fecha.Value.Year == opByKey.fecha.Year && balance1.fecha.Value.Month == opByKey.fecha.Month
                                    && balance1.fecha.Value.Day > opByKey.fecha.Day))
                                {
                                    balan.ingreso -= monto;
                                    balan.efectivo_caja -= monto;
                                }

                            }
                            else
                            {
                                // Obtener el producto utilizado y ponerle la cantidad de la operación.
                                producto prod = opByKey.tipo_producto.producto.FirstOrDefault();
                                if (prod == null) continue;
                                prod.cantidad += opByKey.cantidad;

                                // Eliminar costo de inventario.
                                costo = opByKey.costo;
                                submayor_inventario subM_inventario = entities.submayor_inventario.Where(
                                    inv => inv.fecha.Day == opByKey.fecha.Day 
                                        && inv.fecha.Month == opByKey.fecha.Month
                                        && inv.fecha.Year == opByKey.fecha.Year
                                        && inv.credito == costo && inv.debito == null).First();
                                subM_inventario.credito = 0;
                                subM_inventario.debito = 0;
                                subM_inventario.saldo = 0;
                                subM_inventario.descripcion = "Se elimina operación del dia " + subM_inventario.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                subM_inventario.fecha = DateTime.Now;

                                // Eliminar el costo generado por la operacion
                                submayor_costo subM_costo = entities.submayor_costo.Where(
                                    cost => cost.fecha.Day == opByKey.fecha.Day
                                        && cost.fecha.Month == opByKey.fecha.Month
                                        && cost.fecha.Year == opByKey.fecha.Year
                                        && cost.debito == costo && cost.credito == null).First();
                                subM_costo.credito = 0;
                                subM_costo.debito = 0;
                                subM_costo.saldo = 0;
                                subM_costo.descripcion = "Se elimina operación del dia " + subM_costo.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                subM_costo.fecha = DateTime.Now;

                                monto = opByKey.monto;
                                if (opByKey.pendiente == 0)
                                {
                                    // Elimino el registro de Ingreso que genero la operacion
                                    submayor_ingreso subM_ingreso = entities.submayor_ingreso.Where(
                                        ingreso => ingreso.fecha.Day == opByKey.fecha.Day
                                            && ingreso.fecha.Month == opByKey.fecha.Month
                                            && ingreso.fecha.Year == opByKey.fecha.Year
                                            && ingreso.credito == monto && ingreso.debito == null).First();
                                    subM_ingreso.credito = 0;
                                    subM_ingreso.debito = 0;
                                    subM_ingreso.saldo = 0;
                                    subM_ingreso.descripcion = "Se elimina operación del dia " + subM_ingreso.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_ingreso.fecha = DateTime.Now;

                                    // Elimino el monto del efectivo en caja
                                    submayor_efectivo_caja subM_efectivo_caja = entities.submayor_efectivo_caja.Where(
                                        efectivoC => efectivoC.fecha.Day == opByKey.fecha.Day
                                            && efectivoC.fecha.Month == opByKey.fecha.Month
                                            && efectivoC.fecha.Year == opByKey.fecha.Year
                                            && efectivoC.debito == monto && efectivoC.credito == null).First();
                                    subM_efectivo_caja.debito = 0;
                                    subM_efectivo_caja.credito = 0;
                                    subM_efectivo_caja.saldo = 0;
                                    subM_efectivo_caja.descripcion = "Se elimina operación del dia " + subM_efectivo_caja.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_efectivo_caja.fecha = DateTime.Now;

                                }
                                else
                                {
                                    cXc = true;
                                    // Eliminar las cuentas por cobrar asociadas.
                                    while (opByKey.cuentas_por_cobrar.Count > 0)
                                        entities.DeleteObject(opByKey.cuentas_por_cobrar.First());

                                    //Elimino el registro del submayor de cuentas por cobrar
                                    submayor_cuentas_cobrar subM_cuentas_cobrar = entities.submayor_cuentas_cobrar.Where(
                                        cc => cc.fecha.Day == opByKey.fecha.Day
                                            && cc.fecha.Month == opByKey.fecha.Month
                                            && cc.fecha.Year == opByKey.fecha.Year
                                            && cc.debito == monto && cc.credito == null).First();
                                    subM_cuentas_cobrar.debito = 0;
                                    subM_cuentas_cobrar.credito = 0;
                                    subM_cuentas_cobrar.saldo = 0;
                                    subM_cuentas_cobrar.descripcion = "Se elimina operación del dia " + subM_cuentas_cobrar.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_cuentas_cobrar.fecha = DateTime.Now;

                                    // Elimino el registro de Ingreso que genero la operacion
                                    submayor_ingreso subM_ingreso = entities.submayor_ingreso.Where(
                                        ingreso => ingreso.fecha.Day == opByKey.fecha.Day
                                            && ingreso.fecha.Month == opByKey.fecha.Month
                                            && ingreso.fecha.Year == opByKey.fecha.Year
                                            && ingreso.credito == monto && ingreso.debito == null).First();
                                    subM_ingreso.credito = 0;
                                    subM_ingreso.debito = 0;
                                    subM_ingreso.saldo = 0;
                                    subM_ingreso.descripcion = "Se elimina operación del dia " + subM_ingreso.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_ingreso.fecha = DateTime.Now;
                                }

                                Boolean x = false;
                                // Eliminar las operaciones realizadas por terceros.
                                while (opByKey.operacion_3ero.Count > 0)
                                {
                                    var operacion3Ero = opByKey.operacion_3ero.First();
                                    decimal importe = operacion3Ero.valor_operacion_3ero.Value*operacion3Ero.cantidad;
                                    //Voy obteniendo el gasto total de la operacion
                                    gasto += importe;
                                    gasto_3ero += importe;

                                    // Eliminar cuenta por pagar asociadas.
                                    while (opByKey.cuentas_por_pagar.Count > 0)
                                        entities.DeleteObject(opByKey.cuentas_por_pagar.First());

                                    // Elimino las cuentas por pagar para cada operacion con 3eros
                                    submayor_cuentas_por_pagar subM_cuentas_pagar = entities.submayor_cuentas_por_pagar.Where(
                                        cp => cp.fecha.Day == opByKey.fecha.Day
                                            && cp.fecha.Month == opByKey.fecha.Month
                                            && cp.fecha.Year == opByKey.fecha.Year
                                            && cp.credito == importe && cp.debito == null).First();
                                    subM_cuentas_pagar.credito = 0;
                                    subM_cuentas_pagar.debito = 0;
                                    subM_cuentas_pagar.saldo = 0;
                                    subM_cuentas_pagar.descripcion = "Se elimina operación del dia " + subM_cuentas_pagar.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_cuentas_pagar.fecha = DateTime.Now;
                                 

                                    //Elimino los gastos para cada operacion con terceros
                                    submayor_gasto subM_gasto = entities.submayor_gasto.Where(
                                        sgasto => sgasto.fecha.Day == opByKey.fecha.Day
                                            && sgasto.fecha.Month == opByKey.fecha.Month
                                            && sgasto.fecha.Year == opByKey.fecha.Year
                                            && sgasto.debito == importe && sgasto.credito == null).First();
                                    subM_gasto.credito = 0;
                                    subM_gasto.debito = 0;
                                    subM_gasto.saldo = 0;
                                    subM_gasto.descripcion = "Se elimina operación del dia " + subM_gasto.fecha.ToString("dd/MM/yyyy") + " por el usuario " + _user.nombre;
                                    subM_gasto.fecha = DateTime.Now;

                                    

                                    // Eliminar la operación de 3ero.
                                    entities.DeleteObject(operacion3Ero);
                                    x = true;
                                }

                                if(opByKey.fecha.Date == DateTime.Now)
                                {
                                    // Quitar del salario extra los acumulado por esta operación.
                                    var user =
                                        (usuarios)
                                        entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                              opByKey.id_usuario));
                                    decimal extra = monto * (decimal)user.por_ciento_operaciones / 100;
                                    user.salario_extra_operaciones -= extra;
                                    gasto += extra;

                                    if (opByKey.id_comercial != null && opByKey.id_comercial != 0)
                                    {
                                        var comercial =
                                            (usuarios)
                                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                  opByKey.id_comercial));
                                        extra = monto * (decimal)comercial.por_ciento_operaciones / 100;
                                        comercial.salario_extra_operaciones -= extra;
                                        gasto += extra;
                                    }

                                    if (opByKey.id_operador != null && opByKey.id_operador != 0)
                                    {
                                        var operador =
                                            (usuarios)
                                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                  opByKey.id_operador));
                                        extra = monto * (decimal)operador.por_ciento_operaciones / 100;
                                        operador.salario_extra_operaciones -= extra;
                                        gasto += extra;
                                    }

                                    if (opByKey.id_disenador != null && opByKey.id_disenador != 0)
                                    {
                                        var operador =
                                            (usuarios)
                                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                  opByKey.id_disenador));
                                        extra = monto * (decimal)operador.por_ciento_operaciones / 100;
                                        operador.salario_extra_operaciones -= extra;
                                        gasto += extra;
                                    }
                                }
                                else
                                {
                                    decimal extra = 0;
                                    var user =
                                        (usuarios)
                                        entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                              opByKey.id_usuario));
                                    if(user.id != 1)
                                    {
                                        extra = monto * (decimal)user.por_ciento_operaciones / 100;
                                        submayor_nomina subM_nomina = entities.submayor_nomina.Where(
                                            snomina => snomina.fecha.Day == opByKey.fecha.Day
                                                && snomina.fecha.Month == opByKey.fecha.Month
                                                && snomina.fecha.Year == opByKey.fecha.Year
                                                && snomina.ultimo_pago == user.id).First();
                                        subM_nomina.credito -= extra;
                                        subM_nomina.saldo -= extra;
                                    }
                                    if (opByKey.id_comercial != null && opByKey.id_comercial != 0)
                                    {
                                        var comercial =
                                            (usuarios)
                                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                  opByKey.id_comercial));
                                        extra = monto * (decimal)comercial.por_ciento_operaciones / 100;
                                        submayor_nomina subM_nomina = entities.submayor_nomina.Where(
                                            snomina => snomina.fecha.Day == opByKey.fecha.Day
                                                && snomina.fecha.Month == opByKey.fecha.Month
                                                && snomina.fecha.Year == opByKey.fecha.Year
                                                && snomina.ultimo_pago == comercial.id).First();
                                        subM_nomina.credito -= extra;
                                        subM_nomina.saldo -= extra;
                                    }
                                    if (opByKey.id_operador != null && opByKey.id_operador != 0)
                                    {
                                        var operador =
                                           (usuarios)
                                           entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                 opByKey.id_operador));
                                        extra = monto * (decimal)operador.por_ciento_operaciones / 100;
                                        submayor_nomina subM_nomina = entities.submayor_nomina.Where(
                                            snomina => snomina.fecha.Day == opByKey.fecha.Day
                                                && snomina.fecha.Month == opByKey.fecha.Month
                                                && snomina.fecha.Year == opByKey.fecha.Year
                                                && snomina.ultimo_pago == operador.id).First();
                                        subM_nomina.credito -= extra;
                                        subM_nomina.saldo -= extra;
                                    }
                                    if (opByKey.id_disenador != null && opByKey.id_disenador != 0)
                                    {
                                        var disennador =
                                            (usuarios)
                                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                                  opByKey.id_disenador));
                                        extra = monto * (decimal)disennador.por_ciento_operaciones / 100;
                                        submayor_nomina subM_nomina = entities.submayor_nomina.Where(
                                           snomina => snomina.fecha.Day == opByKey.fecha.Day
                                               && snomina.fecha.Month == opByKey.fecha.Month
                                               && snomina.fecha.Year == opByKey.fecha.Year
                                               && snomina.ultimo_pago == disennador.id).First();
                                        subM_nomina.credito -= extra;
                                        subM_nomina.saldo -= extra;
                                    }
                                }
                                

                                // Eliminar los avisos asociados a la operación.
                                while (opByKey.avisos.Count > 0)
                                    entities.DeleteObject(opByKey.avisos.First());



                                //Elimino de cada balance las utilidades que represento la operacion
                                foreach (var balan in entities.balance.Where(balance1 => balance1.fecha.Value.Year == opByKey.fecha.Year
                                    && balance1.fecha.Value.Month == opByKey.fecha.Month && balance1.fecha.Value.Day == opByKey.fecha.Day
                                    || balance1.fecha.Value.Year == opByKey.fecha.Year && balance1.fecha.Value.Month == opByKey.fecha.Month
                                    && balance1.fecha.Value.Day > opByKey.fecha.Day
                                    || balance1.fecha.Value.Year == opByKey.fecha.Year && balance1.fecha.Value.Month > opByKey.fecha.Month
                                    || balance1.fecha.Value.Year > opByKey.fecha.Year))
                                {
                                    balan.ingreso -= monto;
                                    balan.gasto -= gasto;
                                    balan.costo -= costo;
                                    balan.inventario += costo;

                                    if (cXc)
                                        balan.cuentas_por_cobrar -= monto;
                                    else
                                    {
                                        balan.efectivo_caja -= monto;
                                    }
                                    if (x)
                                        balan.cuentas_por_pagar -= gasto_3ero;
                                }
                            }

                        }

                     

                        // Registrar en la bitácora la eliminación.
                        entities.AddTobitacora(new bitacora
                                                   {
                                                       id_usuario = _user.id,
                                                       nombre_usuario = _user.login_nombre,
                                                       fecha = DateTime.Now,
                                                       accion_realizada = "Eliminada la " + OperacionToString(opByKey)
                                                   });

                        // Elimino la operación.
                        entities.DeleteObject(opByKey);
                    }
                    entities.SaveChanges();
                }
                
                ListarOperaciones();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Eliminar operaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
