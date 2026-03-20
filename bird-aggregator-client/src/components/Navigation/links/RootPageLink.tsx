import { NavHeading } from '../NavStyled'
import React from 'react'

export const RootPageLink: React.FC<{ userName: string }> = ({ userName }) => (
	<NavHeading to="/" title="Main page">
		<b>{userName}</b>
	</NavHeading>
)
