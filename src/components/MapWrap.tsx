import * as React from "react"
import UnderConstructionState from './UnderConstructionState'
import axios from 'axios';
import { Map, MarkerGroup } from 'react-d3-map'
import { ZoomControl } from 'react-d3-map-core'

interface MapWrapProps {}
interface MapWrapState {
    markers: MapMarkerDto[],
    scale: number
}
interface MapMarkerDto {
    id: number,
    x: number,
    y: number,
    birdNames: string
}

export class MapWrap extends React.Component<MapWrapProps, MapWrapState> {
    constructor(props) {
        super(props);
        this.state = {
            markers: [],
            scale: 1 << 14
        };
    }
    componentDidMount() {
        axios.get(`/api/birds/map/markers`).then(res => {
            const markers = res.data as MapMarkerDto[];
            this.setState({ markers });
        });
    }
    zoomOut() {
        this.setState({
          scale: this.state.scale / 2
        })
    }
    zoomIn() {
        this.setState({
          scale: this.state.scale * 2
        })
    }
    render() {
        const onMarkerClick = function(component, d, i) {
            component.showPopup();
        }
        const onMarkerCloseClick = function(component, id) {
            component.hidePopup();
        }

        return (
<div className="body">
    <Map
        width= {window.innerWidth}
        height= {window.innerHeight}
        zoomScale= {this.state.scale}
        scale= { 1 << 14 }
        scaleExtent= {[1 << 10, 1 << 24]}
        center= {[35.5, 55.6]}
    >
    { this.state.markers.map(x => (
        <MarkerGroup
            key= {x.id}
            data= {{
                    "type": "Feature",
                    "properties": {
                        "text": x.birdNames
                    },
                    "geometry": {
                        "type": "Point",
                        "coordinates": [x.x, x.y]
                    }
                }}
            popupContent= { d => d.properties.text }
            markerClass= {"your-marker-css-class"}
            onClick= {onMarkerClick}
            onCloseClick= {onMarkerCloseClick}
        />
    ))}
    <ZoomControl
            zoomInClick= {this.zoomIn}
            zoomOutClick= {this.zoomOut}
          />
  </Map>
</div>);
    }
}