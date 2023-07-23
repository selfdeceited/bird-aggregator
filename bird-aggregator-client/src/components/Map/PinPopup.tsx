import React, { FC } from 'react'
import { BirdPopup } from './BirdPopup'
import { MapMarker } from './types'

type PinProps = { marker: MapMarker }

export const PinPopup: FC<PinProps> = ({ marker }) => <div className="map-popup">
	<button
		style={{ float: 'right' }}
		className="bp3-button bp3-minimal small-reference bp3-icon-cross"
	></button>
	<BirdPopup birds={marker.birds} photoUrl={marker.firstPhotoUrl} />
</div>
