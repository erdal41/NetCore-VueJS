<template>
    <b-card>
        <b-row>
            <b-col cols="12">
                <div class="d-flex align-items-center mb-2">
                    <feather-icon icon="ShieldIcon"
                                  size="18" />
                    <h4 class="mb-0 ml-75">
                       Kullanıcı Rolleri
                    </h4>
                </div>
            </b-col>
        </b-row>
        <b-row>
            <b-form-checkbox v-for="option in localOptions.RoleAssignDtos"
                             :key="option.RoleId"
                             v-model="selected"
                             :value="option.RoleId"
                             name="flavour-3a">
                {{ option.RoleName == 'all.manage' ? 'Yönetici (Tüm Yetki)' : option.RoleName  }}
            </b-form-checkbox>

            <b-form-checkbox-group v-model="selected"
                                   :options="localOptions.RoleAssignDtos"
                                   value-field="RoleId"
                                   text-field="RoleName"
                                   class="demo-inline-spacing" />
            <b-col cols="12">
                <hr class="my-2">
            </b-col>
            <b-col cols="12">
                <b-button variant="primary"
                          class="mt-1 mr-1">
                    Güncelle
                </b-button>
                <b-button type="reset"
                          variant="outline-secondary"
                          class="mt-1"
                          @click.prevent="resetRole">
                    Tüm Değişiklikleri İptal Et
                </b-button>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
    import {
        BButton, BRow, BCol, BCard, BCardHeader, BCardTitle, BFormCheckboxGroup, BFormCheckbox,
    } from 'bootstrap-vue'

    export default {
        components: {
            BButton,
            BRow,
            BCol,
            BCard,
            BCardHeader,
            BCardTitle,
            BFormCheckboxGroup,
            BFormCheckbox
        },
        props: {
            roleData: {
                type: Object,
                default: () => { },
            },
        },
        data() {
            return {
                localOptions: JSON.parse(JSON.stringify(this.roleData)),
                selected: []
            }
        },
        methods: {
            resetRole() {
                this.localOptions = JSON.parse(JSON.stringify(this.roleData))
            },
        },
        mounted() {
        }
    }
</script>
