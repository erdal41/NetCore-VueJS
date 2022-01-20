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
        expireTime: '10m',
        refreshTokenExpireTime: '10m',
    }

    login(request) {
        const userLoginDto = {
            Email: request.email,
            Password: request.password
        }
        return this.axiosIns.post(this.jwtConfig.loginEndpoint, { UserLoginDto: userLoginDto }).then((response) => {
            let error = "Birþeyler ters gitti.";
            if (response.data.UserLoginViewModel != null) {
                try {
                    const accessToken = jwt.sign({ id: response.data.UserLoginViewModel.Id }, this.jwtTokenConfig.secret, { expiresIn: this.jwtTokenConfig.expireTime })
                    const refreshToken = jwt.sign({ id: response.data.UserLoginViewModel.Id }, this.jwtTokenConfig.refreshTokenSecret, {
                        expiresIn: this.jwtTokenConfig.refreshTokenExpireTime,
                    })
                    const userData = response.data.UserLoginViewModel
                    console.log("userData");
                    console.log(response.data.User);
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
