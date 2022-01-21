<template>
  <b-card>
    <!-- form -->
    <b-form>
        <b-row>
            <b-col cols="12">
                <div class="d-flex align-items-center mb-2">
                    <feather-icon icon="InfoIcon"
                                  size="18" />
                    <h4 class="mb-0 ml-75">
                        Kişisel Bilgiler
                    </h4>
                </div>
            </b-col>
            <!-- bio -->
            <b-col cols="12">
                <b-form-group label="Biyografi"
                              label-for="bio-area">
                    <b-form-textarea id="bio-area"
                                     v-model="localOptions.About"
                                     rows="4"
                                     placeholder="Biyografi bilgilerinizi buraya girebilirsiniz..." />
                </b-form-group>
            </b-col>
            <!--/ bio -->
            <!-- website -->
            <b-col md="6">
                <b-form-group label-for="website"
                              label="Website">
                    <b-form-input id="website"
                                  v-model="localOptions.WebSiteLink"
                                  placeholder="Website adresiniz" />
                </b-form-group>
            </b-col>
            <!--/ website -->
            <!-- phone -->
            <b-col md="6">
                <b-form-group label-for="phone"
                              label="Telefon">
                    <cleave id="phone"
                            v-model="localOptions.PhoneNumber"
                            class="form-control"
                            :raw="false"
                            :options="clevePhone"
                            placeholder="Telefon numaranız" />
                </b-form-group>
            </b-col>
            <!-- phone -->

            <b-col cols="12">
                <hr class="my-2">
            </b-col>

            <b-col cols="12">
                <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                          variant="primary"
                          class="mt-1 mr-1">
                    Güncelle
                </b-button>
                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                          type="reset"
                          class="mt-1 ml-25"
                          variant="outline-secondary"
                          @click.prevent="resetForm">
                    Tüm Değişiklikleri İptal Et
                </b-button>
            </b-col>
        </b-row>
    </b-form>
  </b-card>
</template>

<script>
import {
  BButton, BForm, BFormGroup, BFormInput, BRow, BCol, BCard, BFormTextarea,
} from 'bootstrap-vue'
import vSelect from 'vue-select'
import flatPickr from 'vue-flatpickr-component'
import Ripple from 'vue-ripple-directive'
import Cleave from 'vue-cleave-component'

// eslint-disable-next-line import/no-extraneous-dependencies
import 'cleave.js/dist/addons/cleave-phone.us'

export default {
  components: {
    BButton,
    BForm,
    BFormGroup,
    BFormInput,
    BRow,
    BCol,
    BCard,
    BFormTextarea,
    vSelect,
    flatPickr,
    Cleave,
  },
  directives: {
    Ripple,
  },
  props: {
    informationData: {
      type: Object,
      default: () => {},
    },
  },
  data() {
    return {
      countryOption: ['USA', 'India', 'Canada'],
      localOptions: JSON.parse(JSON.stringify(this.informationData)),
      clevePhone: {
        phone: true,
        phoneRegionCode: 'US',
      },
    }
  },
  methods: {
    resetForm() {
      this.localOptions = JSON.parse(JSON.stringify(this.informationData))
    },
  },
}
</script>

<style lang="scss">
@import '@core/scss/vue/libs/vue-select.scss';
@import '@core/scss/vue/libs/vue-flatpicker.scss';
</style>
