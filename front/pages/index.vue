<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <div>
        <v-select rounded outlined label="Seleccione la accion que desea realizar: " :items="actions" :item-value="actions.value" clearable @input="action(value)" v-model="value">
        </v-select>
        <v-select v-if="requireHospital" rounded outlined label="Hospital" :items="hospitales" item-text="nombre"  clearable>
        </v-select>
        <v-select v-if="requireCama" rounded outlined label="Cama" clearable></v-select>
      </div>
      <div class="d-flex justify-center">
        <p class="text-h3">
          o
        </p>
      </div>
      <v-row class="d-flex justify-center">
        <v-col cols="12" class="d-flex justify-center">
          Tipee en la consola lo que desea ejecutar
        </v-col>
        <v-col cols="12">
          <v-textarea background-color="blue" class="text--white" outlined clearable>
          </v-textarea>
        </v-col>
      </v-row>
    </v-col>
  </v-row>
</template>
<script lang="ts">
import Vuetify from "vuetify";
import Vue from "vue";
import  * as SignalR from '@microsoft/signalr';

export default Vue.extend({
  data: () => ({
    hospitales: [],
    SignalRConnection: null,
    value:0,
    requireHospital: false,
    requireCama: false,
    actions: [
      {text: '1-Ver Estado de una cama especifica',value: 1, requireHospital: true, requireCama: true,},
      {text: '2-Ver Estado de las camas de un hospital',value: 2, requireHospital: true, requireCama: false },
      {text: '3-Ver Estado de todas las camas',value: 3, requireHospital: false, requireCama: false },
      {text: '4-Agregar una cama', value: 4, requireHospital: true, requireCama: false, code: "CrearCama"},
      {text: '5-Eliminar una cama', value: 5, requireHospital: true, requireCama: true, code: "EliminarCama"},
      {text: '6-Ocupar una Cama', value: 6, requireHospital: true, requireCama: false, code: "OcuparCama"},
      {text: '7-Desocupar una cama', value: 7, requireHospital: true, requireCama: true, code: "DesocuparCama"},
      {text: '8-Ver el estado de un hospital', value: 8, requireHospital: true, requireCama: false, code:""},
      {text: '9-Ver el estado de todos los hospitales', value: 9, requireHospital: false, requireCama: false}
    ]
  }),
  methods: {
  action(algo: any){
      console.log(algo)
      this.requireCama = this.actions[algo-1].requireCama
      this.requireHospital = this.actions[algo-1].requireHospital
    }
  },
  async mounted() {
    let SignalRConnection = new SignalR.HubConnectionBuilder()
    .withUrl('https://localhost:5001/socket')
    .build()

    await SignalRConnection.start();
    SignalRConnection.invoke("VerEstadoActual")


    SignalRConnection.on("EstadoRecibido", data => {
      console.log("Estado recibido: ", data);
      this.hospitales = data
    });
    SignalRConnection.on("CamaCreada", data => {
      console.log("Cama creada: ", data);
    });
    SignalRConnection.on("CamaEliminada", data => {
      console.log("Cama Eliminada",data);
    });
    SignalRConnection.on("CamaOcupada", data => {
      console.log("Cama ocupada",data);
    });

  }
})
</script>
