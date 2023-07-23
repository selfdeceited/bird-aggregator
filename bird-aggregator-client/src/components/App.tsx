import * as React from 'react'

import { MainRouter } from './MainRouter'
import { Navigation } from './Navigation/Navigation'

export const App: React.FC = () => (<div>
	<Navigation/>
	<MainRouter />
</div>)

// if you need, you can put <SeedProgress/> back after Navbar to switch to the old backend.
// also, change .env to http://localhost:5002
