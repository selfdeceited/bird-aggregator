import { Github } from 'lucide-react'
import { NavAButton } from '../NavStyled'
import React from 'react'

export const GithubLink: React.FC<{ githubLink: string }> = ({ githubLink }) => (
	<NavAButton href={githubLink}>
		<Github size={14}/> GitHub
	</NavAButton>
)
