import * as React from 'react'

import { MainRouter } from './MainRouter'
import { Navbar } from './Navbar/Navbar'

export const App: React.FC = () => (<div>
	<Navbar/>
	<MainRouter />
</div>)

// if you need, you can put <SeedProgress/> back after Navbar to switch to the old backend.
// also, change .env to http://localhost:5002
