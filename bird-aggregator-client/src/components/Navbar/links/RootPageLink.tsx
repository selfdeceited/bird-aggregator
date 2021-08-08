import { Link } from 'react-router-dom'
import React from 'react'

export const RootPageLink: React.FC<{userName: string}> = ({ userName }) => (
	<Link
		to="/" title="Main page" className="bp3-navbar-heading">
		<b>{userName}</b>
	</Link>)
