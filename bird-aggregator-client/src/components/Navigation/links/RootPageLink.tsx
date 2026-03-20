import React from 'react'
import { NavHeading } from '../NavStyled'

export const RootPageLink: React.FC<{ userName: string }> = ({ userName }) => (
	<NavHeading to="/" title="Main page">
		<b>{userName}</b>
	</NavHeading>
)
