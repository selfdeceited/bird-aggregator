import 'yet-another-react-lightbox/styles.css'
import 'yet-another-react-lightbox/plugins/captions.css'
import './index.scss'

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
