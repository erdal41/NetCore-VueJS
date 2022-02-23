<template>
    <div>
        <b-modal id="upload-modal"
                 cancel-variant="outline-secondary"
                 ok-title="Güncelle"
                 cancel-title="İptal"
                 centered
                 size="xl"
                 title="Medya Düzenle"
                 :no-close-on-backdrop="true"
                 @show="getData">
            <b-row>
                <b-col cols="8">a</b-col>
                <b-col cols="4">a</b-col>
            </b-row>
        </b-modal>
    </div>

</template>

<script>
    import {
        BRow, BCol, BForm, BFormGroup, BFormInput, BFormTextarea
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'

    export default {
        components: {
            BRow,
            BCol,
            BForm,
            BFormGroup,
            BFormInput,
            BFormTextarea,
            ToastificationContent
        },
        props: {
            uploadId: {
                type: Number,
                required: true,
            },
        },
        data() {
            return {
                urlRedirectUpdateDto: {
                    Id: this.urlredirectId,
                    OldUrl: "",
                    NewUrl: "",
                    Description: '',
                },
            }
        },
        methods: {
            getData() {
                axios.get('/admin/upload-edit', {
                    params: {
                        uploadId: this.uploadId
                    }
                }).then((response) => {
                    console.log(response.data);
                    
                })
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        if (!this.urlRedirectUpdateDto.OldUrl.includes('https://') && !this.urlRedirectUpdateDto.OldUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Eski url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else if (!this.urlRedirectUpdateDto.NewUrl.includes('https://') && !this.urlRedirectUpdateDto.NewUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Yeni url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else {
                            axios.post('/admin/urlredirect-edit',
                                {
                                    UrlRedirectUpdateDto: this.urlRedirectUpdateDto
                                })
                                .then((response) => {
                                    console.log(response.data);
                                    if (response.data.UrlRedirectDto.ResultStatus === 0) {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'success',
                                                title: 'Başarılı İşlem!',
                                                icon: 'CheckIcon',
                                                text: this.urlRedirectUpdateDto.OldUrl + " linki " + this.urlRedirectUpdateDto.NewUrl + " linkine yönlendirildi."
                                            }
                                        });
                                    }
                                    else {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'danger',
                                                title: 'Başarısız İşlem!',
                                                icon: 'AlertOctagonIcon',
                                                text: response.data.UrlRedirectDto.Message
                                            },
                                        });
                                    }
                                })
                                .catch((error) => {
                                    console.log(error);
                                    console.log(error.request);
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Hata Oluştu!',
                                            icon: 'AlertOctagonIcon',
                                            text: 'Hata oluştu. Lütfen tekrar deneyiniz.',
                                        },
                                    })
                                });
                        }
                    }
                })
            },
        },
        mounted() {
        }
    }
</script>