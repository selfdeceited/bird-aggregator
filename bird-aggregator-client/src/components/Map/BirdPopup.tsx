import { ArrowRight } from 'lucide-react'
import { Link } from 'react-router-dom'
import React from 'react'
import { ShortBirdInfo } from './types'
import styled from 'styled-components'

const Photo = styled.img`
  display: block;
  width: 100%;
  max-width: 220px;
  border-radius: 4px;
  margin-bottom: 8px;
`

const Label = styled.p`
  margin: 0 0 6px;
  font-size: 12px;
  color: #5c7080;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
`

const BirdLinkItem = styled(Link)`
  display: flex;
  align-items: center;
  gap: 5px;
  padding: 3px 0;
  color: #182026;
  text-decoration: none;
  font-size: 13px;
  &:hover { color: #106ba3; }
`

interface BirdPopupProps {
	birds: ShortBirdInfo[]
	photoUrl: string
}

export const BirdPopup: React.FC<BirdPopupProps> = ({ birds, photoUrl }) => (
	<div>
		{photoUrl && (
			<Photo src={photoUrl} alt={birds.map(b => b.name).join(', ')} />
		)}
		<Label>Birds in this photo:</Label>
		{birds.map(bird => (
			<BirdLinkItem key={bird.id} to={`/birds/${bird.id}`}>
				<ArrowRight size={12} />
				{bird.name}
			</BirdLinkItem>
		))}
	</div>
)
