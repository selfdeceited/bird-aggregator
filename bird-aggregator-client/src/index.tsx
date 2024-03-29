import './index.scss'

import * as serviceWorker from './serviceWorker'

import { App } from './components/App'
import { HashRouter } from 'react-router-dom'
import React from 'react'

import { createRoot } from 'react-dom/client'


const container = document.getElementById('root')
// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
const root = createRoot(container!)
root.render(<HashRouter>
	<App/>
</HashRouter>)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister()
