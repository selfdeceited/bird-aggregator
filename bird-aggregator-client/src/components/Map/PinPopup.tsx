import { BirdPopup } from './BirdPopup'
import { MapMarker } from './types'
import React from 'react'
import styled from 'styled-components'

const Container = styled.div`
  padding: 4px 2px;
  min-width: 180px;
  max-width: 240px;
`

type PinProps = { marker: MapMarker }

export const PinPopup: React.FC<PinProps> = ({ marker }) => (
	<Container>
		<BirdPopup birds={marker.birds} photoUrl={marker.firstPhotoUrl} />
	</Container>
)
