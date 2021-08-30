<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <div>
        <v-select rounded outlined label="Seleccione la accion que desea realizar: " :items="actions" :item-value="actions.value" clearable>
        </v-select>
        <v-select rounded outlined label="Hospital" clearable>
        </v-select>
        <v-select rounded outlined label="Cama" clearable></v-select>
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
    actions: [
      {text: 'Ver Estado de una cama',value: 1 },
      {text: 'Agregar una cama', value: 2},
      {text: 'Eliminar una cama', value: 3},
      {text: 'Ver el estado de un hospital', value: 4},
      {text: 'Ver el estado de todos los hospitales', value: 5}
    ]
  }),
  mounted() {
    let SignalRConnection = new SignalR.HubConnectionBuilder()
    .withUrl('https://localhost:5001/socket')
    .build()

    SignalRConnection.on("EstadoRecibido", data => {
      console.log(data);
    });
    SignalRConnection.start()
      .then(() => SignalRConnection.invoke("VerEstadoActual"));
  }
})
</script>
