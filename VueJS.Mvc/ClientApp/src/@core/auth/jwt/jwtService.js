import jwtDefaultConfig from './jwtDefaultConfig'
import jwt from 'jsonwebtoken'

export default class JwtService {
    // Will be used by this service for making API calls
    axiosIns = null

    // jwtConfig <= Will be used by this service
    jwtConfig = { ...jwtDefaultConfig }

    // For Refreshing Token
    isAlreadyFetchingAccessToken = false

    // For Refreshing Token
    subscribers = []

    constructor(axiosIns, jwtOverrideConfig) {
        this.axiosIns = axiosIns
        this.jwtConfig = { ...this.jwtConfig, ...jwtOverrideConfig }

        // Request Interceptor
        this.axiosIns.interceptors.request.use(
            config => {
                // Get token from localStorage
                const accessToken = this.getToken()
                
                // If token is present add it to request's Authorization Header
                if (accessToken) {
                    // eslint-disable-next-line no-param-reassign
                    console.log('asdasdasdsadsadsadasdsad--------------');
                    console.log(accessToken);
                    console.log(config);
                    config.headers.Authorization = `${this.jwtConfig.tokenType} ${accessToken}`
                }
                return config
            },
            error => Promise.reject(error),
        )

        // Add request/response interceptor
        this.axiosIns.interceptors.response.use(
            response => response,
            error => {
                // const { config, response: { status } } = error
                const { config, response } = error
                const originalRequest = config

                // if (status === 401) {
                if (response && response.status === 401) {
                    if (!this.isAlreadyFetchingAccessToken) {
                        this.isAlreadyFetchingAccessToken = true
                        this.refreshToken().then(r => {
                            this.isAlreadyFetchingAccessToken = false

                            // Update accessToken in localStorage
                            this.setToken(r.data.accessToken)
                            this.setRefreshToken(r.data.refreshToken)

                            this.onAccessTokenFetched(r.data.accessToken)
                        })
                    }
                    const retryOriginalRequest = new Promise(resolve => {
                        this.addSubscriber(accessToken => {
                            // Make sure to assign accessToken according to your response.
                            // Check: https://pixinvent.ticksy.com/ticket/2413870
                            // Change Authorization header
                            originalRequest.headers.Authorization = `${this.jwtConfig.tokenType} ${accessToken}`
                            resolve(this.axiosIns(originalRequest))
                        })
                    })
                    return retryOriginalRequest
                }
                return Promise.reject(error)
            },
        )
    }

    onAccessTokenFetched(accessToken) {
        this.subscribers = this.subscribers.filter(callback => callback(accessToken))
    }

    addSubscriber(callback) {
        this.subscribers.push(callback)
    }

    getToken() {
        return localStorage.getItem(this.jwtConfig.storageTokenKeyName)
    }

    getRefreshToken() {
        return localStorage.getItem(this.jwtConfig.storageRefreshTokenKeyName)
    }

    setToken(value) {
        localStorage.setItem(this.jwtConfig.storageTokenKeyName, value)
    }

    setRefreshToken(value) {
        localStorage.setItem(this.jwtConfig.storageRefreshTokenKeyName, value)
    }

    jwtTokenConfig = {
        secret: 'dd5f3089-40c3-403d-af14-d0c228b05cb4',
        refreshTokenSecret: '7c4c1c50-3230-45bf-9eae-c9b2e401c767',
        expireTime: '10s',
        refreshTokenExpireTime: '10s',
    }

    login1(request) {
        const userLoginDto = {
            Email: request.email,
            Password: request.password,
            RememberMe: request.rememberMe
        }
        return this.axiosIns.post('/admin/auth-login', { UserLoginDto: userLoginDto })
            .then((response) => {
                if (response.data.UserLoginViewModel != null) {
                    console.log('LOGIN!!!');
                    console.log(response.data)
                    const userData = response.data.UserLoginViewModel;
                    const accessToken = response.data.TokenModel.AccessToken;
                    const refreshToken = response.data.TokenModel.RefreshToken;

                    localStorage.setItem('userData', JSON.stringify(userData))
                    localStorage.setItem('accessToken', accessToken);
                    localStorage.setItem('refreshToken', refreshToken);

                    var ability = [];
                    ability.push({ subject: 'Auth', action: 'read' })
                    if (userData != null) {
                        for (let role of userData.Roles) {
                            ability.push({ subject: role.split(".")[0], action: role.split(".")[1] });
                        }
                    }

                    this.$ability.update(ability)
                    this.$router.replace('/admin/dashboard').then(() => {
                        this.$toast({
                            component: ToastificationContent,
                            position: 'top-right',
                            props: {
                                title: `Hoþgeldin ${userData.FirstName || userData.LastName}`,
                                icon: 'CoffeeIcon',
                                variant: 'success',
                                text: 'Baþarýlý bir þekilde giriþ yaptýnýz.',
                            },
                        })
                    })
                } else {
                    this.$toast({
                        component: ToastificationContent,
                        props: {
                            variant: 'danger',
                            title: 'Hata!',
                            icon: 'AlertOctagonIcon',
                            text: 'E-posta adresiniz veya parolanýz yanlýþ olabilir. Lütfen kontrol ediniz.',
                        }
                    })
                }
            }).catch(error => {
                console.log(error)
                console.log(error.request)
                this.$toast({
                    component: ToastificationContent,
                    props: {
                        variant: 'danger',
                        title: 'Hata!',
                        icon: 'AlertOctagonIcon',
                        text: 'Hata oluþtu. Lütfen tekrar deneyiniz.',
                    }
                })
            })
    }

    login(request) {
        const userLoginDto = {
            Email: request.email,
            Password: request.password,
            RememberMe: request.rememberMe
        }
        return this.axiosIns.post(this.jwtConfig.loginEndpoint, { UserLoginDto: userLoginDto }).then((response) => {
            let error = "Birþeyler ters gitti.";
            if (response.data.UserLoginViewModel != null) {
                try {
                    //const accessToken = jwt.sign({ id: response.data.UserLoginViewModel.Id }, this.jwtTokenConfig.secret, { expiresIn: this.jwtTokenConfig.expireTime })
                    //const refreshToken = jwt.sign({ id: response.data.UserLoginViewModel.Id }, this.jwtTokenConfig.refreshTokenSecret, { expiresIn: this.jwtTokenConfig.refreshTokenExpireTime})
                    const userData = response.data.UserLoginViewModel;
                    const accessToken = response.data.TokenModel.AccessToken;
                    const refreshToken = response.data.TokenModel.RefreshToken;
                    console.log("userData");
                    console.log(response.data);
                    //delete request.password
                    const responsed = {
                        userData,
                        accessToken,
                        refreshToken,
                    }
                    return [200, responsed]
                } catch (e) {
                    error = e
                }
            } else {
                error = "E-posta adresiniz veya parolanýz yanlýþ olabilir. Lütfen kontrol ediniz.";
            }
            return [400, { error }]
        })
    }
    //login(...args) {
    //    return this.axiosIns.post(this.jwtConfig.loginEndpoint, ...args)
    //}

    register(...args) {
        return this.axiosIns.post(this.jwtConfig.registerEndpoint, ...args)
    }

    refreshToken() {
        return this.axiosIns.post(this.jwtConfig.refreshEndpoint, {
            refreshToken: this.getRefreshToken(),
        })
    }
}
