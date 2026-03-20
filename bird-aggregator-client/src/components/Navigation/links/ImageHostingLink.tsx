import React from 'react'
import { Images } from 'lucide-react'
import { NavAButton } from '../NavStyled'

export const ImageHostingLink: React.FC<{ imageHostingLink: string }> = ({ imageHostingLink }) => (
	<NavAButton href={imageHostingLink}>
		<Images size={14}/> Flickr
	</NavAButton>
)
